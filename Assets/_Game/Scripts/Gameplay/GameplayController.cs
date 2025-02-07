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

    private void OnInput(OutfitTag outfit)
    {
        if (!IsAvailable) return;

        Color color = webcamController.GetCurrentColor();

        bearController.Play(outfit, color);
    }

    #endregion

}