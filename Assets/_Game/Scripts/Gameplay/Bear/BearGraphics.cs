using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private new SkinnedMeshRenderer renderer;

    [Header("Settings")]
    [SerializeField]
    private List<OutfitData> outfits;

    private readonly int playID = Animator.StringToHash("Play");
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

    public async void SetData(Outfit outfit, List<Color> colors)
    {
        OutfitData data = outfits[(int)outfit];

        renderer.sharedMesh = data.mesh;

        Texture2D texture = await GetOutfitTexture(data, colors);

        properties.SetTexture(mainTextureID, texture);
        renderer.SetPropertyBlock(properties);
    }

    public void Play()
    {
        animator.SetTrigger(playID);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private async Task<Texture2D> GetOutfitTexture(OutfitData outfit, List<Color> colors)
    {
        // Get values
        int width = outfit.albedo.width;
        int height = outfit.albedo.height;

        int size = width * height;
        int layers = Math.Min(outfit.masks.Count, colors.Count);

        // Get containers
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[size];

        float[][] values = await GetMaskValues(outfit.masks);
        Color[] albedos = outfit.albedo.GetPixels();

        // Generate texture
        await Task.Run(() =>
        {
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
        });

        // Apply texture
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    private async Task<float[][]> GetMaskValues(List<Texture2D> masks)
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

            await Task.Yield();
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