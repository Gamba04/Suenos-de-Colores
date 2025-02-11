using System;
using System.Collections.Generic;
using UnityEngine;

public class BearGraphics : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class OutfitData
    {
        [SerializeField, HideInInspector] private string name;

        public Mesh mesh;

        public void SetName(Outfit outfit)
        {
            name = outfit.ToString();
        }
    }

    #endregion

    [Header("Components")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MeshFilter filter;
    [SerializeField]
    private new Renderer renderer;

    [Header("Settings")]
    [SerializeField]
    private List<OutfitData> outfits;

    private readonly int playID = Animator.StringToHash("Play");
    private readonly int animationID = Animator.StringToHash("Animation");

    private readonly int colorsID = Shader.PropertyToID("_Colors");

    private MaterialPropertyBlock properties;

    #region Init

    public void Init()
    {
        properties = new MaterialPropertyBlock();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Play(int animation)
    {
        animator.SetInteger(animationID, animation);
        animator.SetTrigger(playID);
    }

    public void SetData(Outfit outfit, List<Color> colors)
    {
        OutfitData data = outfits[(int)outfit];

        filter.mesh = data.mesh;

        properties.SetVectorArray(colorsID, GetVectors(colors));

        renderer.SetPropertyBlock(properties);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private List<Vector4> GetVectors(List<Color> colors) => colors.ConvertAll(color => new Vector4(color.r, color.g, color.b));

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        outfits.Resize(typeof(Outfit));
        outfits.ForEach((outfit, index) => outfit.SetName((Outfit)index));
    }

#endif

    #endregion

}