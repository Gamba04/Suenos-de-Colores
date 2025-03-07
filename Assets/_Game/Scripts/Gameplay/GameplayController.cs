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
        InitEvents();

        inactivityController.Init();
        bearController.Init();
        walkingBearsController.Init();

        await InitAsync();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;

        bearController.onFinishPlaying += OnFinishPlaying;
    }

    private async Task InitAsync()
    {
        BeginFade();

        await Task.Yield();

        await webcamController.Init();

        EndFade();
    }

    private void BeginFade()
    {
        UIFade.SetFade(true, true);
    }

    private void EndFade()
    {
        UIFade.SetFade(false);

        isInitialized = true;
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