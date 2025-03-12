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
        InitPreAsync();

        await InitAsyncSequence();

        InitPostAsync();
    }

    #region Pre Async

    private void InitPreAsync()
    {
        InitEvents();

        inactivityController.Init();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;
        bearController.onFinishPlaying += OnFinishPlaying;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Async

    private async Task InitAsyncSequence()
    {
        UIFade.SetFade(true, true);

        Task async = InitAsync();

        InitInterAsync();

        await async;

        UIFade.SetFade(false);

        isInitialized = true;
    }

    private async Task InitAsync()
    {
        await Task.Yield();
        await webcamController.Init();
    }

    private void InitInterAsync()
    {
        bearController.Init();
        walkingBearsController.Init();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Post Async

    private void InitPostAsync()
    {
        inactivityController.StartCooldown();
    }

    #endregion

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