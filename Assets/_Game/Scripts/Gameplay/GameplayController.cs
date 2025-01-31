using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WebcamController webcamController;

    private bool pictureTaken;
    private Color pictureColor;

    #region Start

    private void Start()
    {
        InitEvents();

        webcamController.Init();

        TestWebcam(); // Temp
    }

    private void InitEvents()
    {
        webcamController.onPictureTaken += OnPictureTaken;
    }

    private void TestWebcam()
    {
        if (!webcamController.OpenWebcam())
        {
            Debug.Log("No image");
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Picture

    private void OnPictureTaken(Color color)
    {
        pictureTaken = true;
        pictureColor = color;
    }

    #endregion

}