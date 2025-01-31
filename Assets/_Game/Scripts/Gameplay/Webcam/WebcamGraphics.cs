using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class WebcamGraphics : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private RawImage viewport;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RectTransform focusHint;

    [Header("Settings")]
    [Range(0, 1)]
    [SerializeField]
    private float focusRadius = 0.25f;

    public float FocusRadius => focusRadius;

    private readonly int visibleID = Animator.StringToHash("Visible");
    private readonly int takePictureID = Animator.StringToHash("TakePicture");

    #region Init

    public void Init(WebCamTexture texture)
    {
        viewport.texture = texture;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SetGraphics(bool enabled)
    {
        animator.SetBool(visibleID, enabled);
    }

    public void TakePicture()
    {
        animator.SetTrigger(takePictureID);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (focusHint == null) return;

        focusHint.localScale = GambaFunctions.GetScaleOf(focusRadius);
    }

    private void OnDrawGizmos()
    {
        RectTransform root = transform as RectTransform;

        Vector3 position = root.position;
        float radius = focusRadius * root.rect.height * root.lossyScale.y / 2;

        Handles.color = Color.black;
        Handles.DrawWireArc(position, Vector3.forward, Vector3.up, 360, radius);
    }

#endif

    #endregion

}