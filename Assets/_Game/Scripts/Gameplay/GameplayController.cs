using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WebcamController webcamController;
    [SerializeField]
    private BearController bearController;

    private Color pictureColor;

    #region Start

    private void Start()
    {
        InitEvents();

        webcamController.Init();

        UIFade.SetFade(false, onFinishFade: TakePicture); // Temp
    }

    private void InitEvents()
    {
        webcamController.onPictureTaken += OnPictureTaken;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Gameplay

    public void TakePicture()
    {
        if (webcamController.OpenWebcam())
        {
            SetInteractions(false);
        }
        else Debug.Log("No image");
    }

    private void OnPictureTaken(Color color)
    {
        pictureColor = color;

        SetInteractions(true);
        //SetBearButton(true);
    }

    public void PlayBear()
    {
        bearController.Play(pictureColor);

        SetInteractions(false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetInteractions(bool enabled)
    {
        Button.Interactions = enabled;
    }

    #endregion

}