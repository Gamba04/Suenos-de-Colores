using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameplayInput input;
    [SerializeField]
    private WebcamController webcamController;
    [SerializeField]
    private BearController bearController;
    [SerializeField]
    private InactivityController inactivityController;

    public bool IsAvailable => webcamController.IsAvailable && bearController.IsAvailable;

    #region Init

    private void Awake()
    {
        InitEvents();

        webcamController.Init();
        bearController.Init();
        inactivityController.Init();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;

        bearController.onFinishAnim += OnFinishAnim;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Gameplay

    private async void OnInput(Outfit outfit)
    {
        if (!IsAvailable) return;

        await inactivityController.OnStartInteraction();

        List<Color> colors = await webcamController.GetOutfitColors(outfit);

        bearController.Play(outfit, colors);
    }

    private void OnFinishAnim()
    {
        inactivityController.OnFinishInteraction();
    }

    #endregion

}