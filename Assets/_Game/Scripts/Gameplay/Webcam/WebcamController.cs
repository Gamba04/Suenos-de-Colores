using UnityEngine;

public class WebcamController : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 1)]
    [SerializeField]
    private float focusRadius = 0.25f;

    private WebCamTexture webcam;

    public bool IsAvailable => webcam.isPlaying;

    #region Init

    public void Init()
    {
        webcam = new WebCamTexture();
        webcam.Play();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public Color GetCurrentColor()
    {
        return WebcamProcessing.GetColor(webcam, focusRadius);
    }

    #endregion

}