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
        public List<Texture2D> masks;
        public Texture2D albedo;

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

    private readonly int mainTextureID = Shader.PropertyToID("_MainTex");

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

        Texture2D texture = GetOutfitTexture(data, colors);

        properties.SetTexture(mainTextureID, texture);
        renderer.SetPropertyBlock(properties);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Texture2D GetOutfitTexture(OutfitData outfit, List<Color> colors)
    {
        Texture2D texture = new Texture2D(outfit.albedo.width, outfit.albedo.height);

        int layers = Mathf.Min(outfit.masks.Count, colors.Count);

        for (int l = 0; l < layers; l++)
        {
            Texture2D mask = outfit.masks[l];
            Color color = colors[l];

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color pixel = mask.GetPixel(x, y);

                    if (pixel.r == 0) continue;

                    texture.SetPixel(x, y, color);
                }
            }
        }

        texture.Apply();

        return texture;
    }

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