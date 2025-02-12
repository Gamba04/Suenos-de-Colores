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
        int width = outfit.albedo.width;
        int height = outfit.albedo.height;

        int size = width * height;
        int layers = Math.Min(outfit.masks.Count, colors.Count);

        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[size];

        float[][] values = GetMaskValues(outfit.masks);
        Color[] albedos = outfit.albedo.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;

                Color pixel = default;

                for (int l = 0; l < layers; l++)
                {
                    float value = values[l][index];

                    if (value == 0) continue;

                    pixel = Color.Lerp(pixel, colors[l], value);
                }

                pixel *= albedos[index];

                pixels[index] = pixel;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    private float[][] GetMaskValues(List<Texture2D> masks)
    {
        float[][] values = new float[masks.Count][];

        for (int m = 0; m < masks.Count; m++)
        {
            Color32[] maskPixels = masks[m].GetPixels32();
            float[] maskValues = new float[maskPixels.Length];

            for (int i = 0; i < maskPixels.Length; i++)
            {
                maskValues[i] = (float)maskPixels[i].r / byte.MaxValue;
            }

            values[m] = maskValues;
        }

        return values;
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