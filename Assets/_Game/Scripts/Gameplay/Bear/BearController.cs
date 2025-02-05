using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Material colorMaterial;

    private bool isPlaying;

    public bool IsAvailable => !isPlaying;

    #region Public Methods

    public void Play(BearType bear, Color color)
    {
        isPlaying = true;

        colorMaterial.color = color;
    }

    public void AnimFinish()
    {
        isPlaying = true;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        colorMaterial.color = Color.white;
    }

#endif

    #endregion

}