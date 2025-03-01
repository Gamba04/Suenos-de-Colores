using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WebcamController : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class OutfitData
    {
        [SerializeField, HideInInspector] private string name;

        public Color gizmosColor = Color.white;
        public WebcamDataAsset data;

        public void SetName(Outfit outfit)
        {
            string data = this.data != null ? this.data.name : "None";

            name = $"{outfit} ({data})";
        }
    }

    #endregion

    [SerializeField]
    private List<OutfitData> outfits = new List<OutfitData>();

    private WebCamTexture webcam;

    public bool IsAvailable => webcam.isPlaying;

    #region Init

    public void Init()
    {
        webcam = new WebCamTexture();
        webcam.Play();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public async Task<List<Color>> GetOutfitColors(Outfit outfit)
    {
        List<Color> colors = new List<Color>();

        OutfitData data = outfits[(int)outfit];
        Texture2D picture = TakePicture();

        await Task.Yield();

        foreach (WebcamDataAsset.OutfitNode node in data.data.Nodes)
        {
            Color color = WebcamProcessing.ScanColor(picture, node.position, node.size);

            colors.Add(color);

            await Task.Yield();
        }

        return colors;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Texture2D TakePicture()
    {
        Texture2D texture = new Texture2D(webcam.width, webcam.height, TextureFormat.RGBA32, false);

        texture.SetPixels32(webcam.GetPixels32());

        texture.Apply();

        return texture;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        RectTransform root = transform as RectTransform;

        float height = root.rect.height * root.lossyScale.y;

        foreach (OutfitData outfit in outfits)
        {
            if (outfit.data == null) continue;

            Gizmos.color = outfit.gizmosColor;

            foreach (WebcamDataAsset.OutfitNode node in outfit.data.Nodes)
            {
                Vector3 position = node.position * height;
                float size = node.size * height;

                Gizmos.DrawWireCube(transform.position + position, Vector2.one * size);
            }
        }
    }

    private void OnValidate()
    {
        outfits.Resize(typeof(Outfit));
        outfits.ForEach((outfit, index) => outfit.SetName((Outfit)index));
    }

#endif

    #endregion

}