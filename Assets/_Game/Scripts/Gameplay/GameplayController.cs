using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameplayInput input;
    [SerializeField]
    private WebcamController webcamController;
    [SerializeField]
    private InactivityController inactivityController;
    [SerializeField]
    private BearController bearController;
    [SerializeField]
    private WalkingBearsController walkingBearsController;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private bool isPlaying;

    private bool isInitialized;

    private bool IsAvailable => isInitialized && !isPlaying;

    #region Init

    private async void Awake()
    {
        await BeginInit();

        InitEvents();

        inactivityController.Init();
        bearController.Init();
        walkingBearsController.Init();

        await webcamController.Init();

        EndInit();
    }

    private async Task BeginInit()
    {
        UIFade.SetFade(true, true);

        await Task.Yield();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;

        bearController.onFinishPlaying += OnFinishPlaying;
    }

    private void EndInit()
    {
        isInitialized = true;

        UIFade.SetFade(false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Gameplay

    private async void OnInput(Outfit outfit)
    {
        if (!IsAvailable) return;

        isPlaying = true;

        Task stopInactivity = inactivityController.StopInactivity();

        List<Color> colors = await webcamController.GetOutfitColors(outfit);

        await bearController.SetData(outfit, colors);

        await stopInactivity;

        bearController.Play();
    }

    private void OnFinishPlaying()
    {
        inactivityController.StartCooldown();
        walkingBearsController.SpawnBear(bearController.Data);

        isPlaying = false;
    }

    #endregion

}