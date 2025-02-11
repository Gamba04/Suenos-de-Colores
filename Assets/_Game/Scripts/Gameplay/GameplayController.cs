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

    public bool IsAvailable => webcamController.IsAvailable && bearController.IsAvailable;

    #region Init

    private void Awake()
    {
        InitEvents();

        webcamController.Init();
        bearController.Init();
    }

    private void InitEvents()
    {
        input.onInput += OnInput;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Gameplay

    private void OnInput(Outfit outfit)
    {
        if (!IsAvailable) return;

         List<Color> colors = webcamController.GetOutfitColors(outfit);

        bearController.Play(outfit, colors);
    }

    #endregion

}