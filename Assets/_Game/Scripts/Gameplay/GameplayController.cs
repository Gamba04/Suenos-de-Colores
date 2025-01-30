using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WebcamController webcamController;

    #region Start

    private void Start()
    {
        webcamController.Init();

        TestWebcam(); // Temp
    }

    private void TestWebcam()
    {
        if (!webcamController.OpenWebcam())
        {
            Debug.Log("No image");
        }
    }

    #endregion

}