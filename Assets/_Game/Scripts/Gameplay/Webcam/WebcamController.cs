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

        [Space]
        public List<OutfitNode> nodes = new List<OutfitNode>();

        public void SetName(Outfit outfit)
        {
            name = outfit.ToString();

            nodes.ForEach((node, index) => node.SetName(index));
        }
    }

    [Serializable]
    private class OutfitNode
    {
        [SerializeField, HideInInspector] private string name;

        public Vector2 position;

        [Range(0, 1)]
        public float size;

        public void SetName(int index)
        {
            name = $"Node {index}";
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

        await Task.Yield();

        foreach (OutfitNode node in data.nodes)
        {
            Color color = WebcamProcessing.ScanColor(webcam, node.position, node.size);

            colors.Add(color);

            await Task.Yield();
        }

        return colors;
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
            Gizmos.color = outfit.gizmosColor;

            foreach (OutfitNode node in outfit.nodes)
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