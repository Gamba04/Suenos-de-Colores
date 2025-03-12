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
        public Texture2D normal;

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
    private readonly int mainTextureID = Shader.PropertyToID("_BaseMap");
    private readonly int normalMapID = Shader.PropertyToID("_BumpMap");

    private readonly Texture2D[] outfitTextures = new Texture2D[OutfitsAmount];
    private readonly Color[][] outfitPixels = new Color[OutfitsAmount][];
    private readonly float[][][] outfitMasks = new float[OutfitsAmount][][];
    private readonly Color[][] outfitAlbedos = new Color[OutfitsAmount][];

    private MaterialPropertyBlock properties;

    public SkinnedMeshRenderer Renderer => renderer;

    private static int OutfitsAmount => Enum.GetValues(typeof(Outfit)).Length;

    #region Init

    public void Init()
    {
        properties = new MaterialPropertyBlock();

        InitOutfitContainers();
        InitOutfitData();
    }

    private void InitOutfitContainers()
    {
        for (int i = 0; i < outfitTextures.Length; i++)
        {
            OutfitData outfit = outfits[i];

            bool hasAlbedo = outfit.albedo != null;
            Texture2D reference = hasAlbedo ? outfit.albedo : outfit.masks[0];

            int size = reference.width * reference.height;

            outfitTextures[i] = new Texture2D(reference.width, reference.height);
            outfitPixels[i] = new Color[size];
        }
    }

    private void InitOutfitData()
    {
        for (int i = 0; i < outfitMasks.Length; i++)
        {
            OutfitData outfit = outfits[i];

            bool hasAlbedo = outfit.albedo != null;

            outfitMasks[i] = GetMaskValues(outfit);
            outfitAlbedos[i] = hasAlbedo ? outfit.albedo.GetPixels() : null;
        }
    }

    private float[][] GetMaskValues(OutfitData outfit)
    {
        List<Texture2D> masks = outfit.masks;
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

    #region Public Methods

    public async Task SetData(int outfit, List<Color> colors)
    {
        OutfitData data = outfits[outfit];

        renderer.sharedMesh = data.mesh;

        Texture2D texture = await GetOutfitTexture(outfit, data, colors);

        properties.SetTexture(mainTextureID, texture);
        properties.SetTexture(normalMapID, data.normal);

        renderer.SetPropertyBlock(properties);
    }

    public void Play()
    {
        animator.SetTrigger(playID);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private async Task<Texture2D> GetOutfitTexture(int outfit, OutfitData data, List<Color> colors)
    {
        // Get preprocessed data
        Texture2D texture = outfitTextures[outfit];
        Color[] pixels = outfitPixels[outfit];
        float[][] values = outfitMasks[outfit];
        Color[] albedos = outfitAlbedos[outfit];

        // Get values
        int width = texture.width;
        int height = texture.height;

        int layers = Math.Min(data.masks.Count, colors.Count);

        // Generate texture
        await Task.Run(() =>
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    Color pixel = Color.white;

                    for (int l = 0; l < layers; l++)
                    {
                        float value = values[l][index];

                        if (value == 0) continue;

                        pixel = Color.Lerp(pixel, colors[l], value);
                    }

                    if (albedos != null) pixel *= albedos[index];

                    pixels[index] = pixel;
                }
            }
        });

        // Apply texture
        texture.SetPixels(pixels);

        await Task.Yield();

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