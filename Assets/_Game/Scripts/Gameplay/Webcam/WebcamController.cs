using System;
using UnityEngine;

public class WebcamController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WebcamInput input;
    [SerializeField]
    private WebcamGraphics graphics;

    [Header("Settings")]
    [SerializeField]
    private float previewDelay = 2;

    private WebCamTexture webcam;

    public event Action<Color> onPicture;

    #region Init

    public void Init()
    {
        InitEvents();
        InitWebcam();
    }

    private void InitEvents()
    {
        input.onTrigger += TakePicture;
    }

    private void InitWebcam()
    {
        webcam = new WebCamTexture();

        graphics.Init(webcam);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public bool OpenWebcam()
    {
        if (CheckWebcam())
        {
            SetWebcam(true);
            SetGraphics(true);

            return true;
        }

        return false;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Process

    private void TakePicture()
    {
        graphics.TakePicture();

        SetWebcam(false);

        Timer.CallOnDelay(CloseWebcam, previewDelay, "Preview delay");
    }

    private void CloseWebcam()
    {
        SetGraphics(false);
        
        Color targetColor = WebcamProcessing.GetTargetColor(webcam, graphics.FocusRadius);

        onPicture?.Invoke(targetColor);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private bool CheckWebcam()
    {
        webcam.Play();

        return webcam.isPlaying;
    }

    private void SetWebcam(bool enabled)
    {
        if (enabled) webcam.Play();
        else webcam.Stop();

        input.SetInput(enabled);
    }

    private void SetGraphics(bool enabled)
    {
        graphics.SetGraphics(enabled);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        previewDelay = Mathf.Max(previewDelay, 0);
    }

#endif

    #endregion

}