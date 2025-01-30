using UnityEngine;
using UnityEngine.UI;

public class WebcamGraphics : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private RawImage viewport;
    [SerializeField]
    private Animator animator;

    [Header("Settings")]
    [Range(0, 1)]
    [SerializeField]
    private float focusRadius = 0.25f;

    public float FocusRadius => focusRadius;

    private readonly int visibleID = Animator.StringToHash("Visible");
    private readonly int takePictureID = Animator.StringToHash("TakePicture");

    public void Init(WebCamTexture texture)
    {
        viewport.texture = texture;
    }

    public void SetGraphics(bool enabled)
    {
        animator.SetBool(visibleID, enabled);
    }

    public void TakePicture()
    {
        animator.SetTrigger(takePictureID);
    }
}