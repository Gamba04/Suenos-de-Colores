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

        public void SetName(OutfitTag outfit)
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

    private readonly int colorID = Shader.PropertyToID("_Color");

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

    public void SetOutfit(OutfitTag outfit)
    {
        OutfitData data = outfits[(int)outfit];

        filter.mesh = data.mesh;
    }

    public void SetColor(Color color)
    {
        properties.SetColor(colorID, color);
        renderer.SetPropertyBlock(properties);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        outfits.Resize(typeof(OutfitTag));
        outfits.ForEach((outfit, index) => outfit.SetName((OutfitTag)index));
    }

#endif

    #endregion

}