using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Material colorMaterial;

    #region Public Methods

    public void Play(Color color)
    {
        colorMaterial.color = color;
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