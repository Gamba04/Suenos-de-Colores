using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WebcamController webcamController;

    private bool pictureTaken;
    private Color targetColor;

    #region Start

    private void Start()
    {
        InitEvents();

        webcamController.Init();

        TestWebcam(); // Temp
    }

    private void InitEvents()
    {
        webcamController.onPicture += OnPicture;
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

    private void OnPicture(Color color)
    {
        pictureTaken = true;
        targetColor = color;
    }

    #endregion

}