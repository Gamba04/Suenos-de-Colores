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

    public bool IsAvailable => !isPlaying && webcamController.IsAvailable;

    #region Init

    private void Awake()
    {
        InitEvents();

        webcamController.Init();
        inactivityController.Init();
        bearController.Init();
        walkingBearsController.Init();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;

        bearController.onFinishPlaying += OnFinishPlaying;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Gameplay

    private async void OnInput(Outfit outfit)
    {
        if (!IsAvailable) return;

        isPlaying = true;

        await Task.Yield();

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