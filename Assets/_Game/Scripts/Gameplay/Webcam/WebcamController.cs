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
    private Texture2D picture;

    public event Action<Color> onPictureTaken;

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

    #region Picture

    private void TakePicture()
    {
        graphics.TakePicture();

        SavePicture();
        SetWebcam(false);

        Timer.CallOnDelay(CloseWebcam, previewDelay, "Preview delay");
    }

    private void SavePicture()
    {
        Vector2Int size = new Vector2Int(webcam.width, webcam.height);

        picture = new Texture2D(size.x, size.y);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Color color = webcam.GetPixel(x, y);

                picture.SetPixel(x, y, color);
            }
        }

        picture.Apply();
    }

    private void CloseWebcam()
    {
        SetGraphics(false);

        Color color = WebcamProcessing.GetPictureColor(picture, graphics.FocusRadius);

        onPictureTaken?.Invoke(color);
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