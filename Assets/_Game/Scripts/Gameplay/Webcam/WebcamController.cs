using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        [Range(0, 0.5f)]
        public float radius;

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

    public List<Color> GetOutfitColors(Outfit outfit)
    {
        List<Color> colors = new List<Color>();

        OutfitData data = outfits[(int)outfit];

        foreach (OutfitNode node in data.nodes)
        {
            Color color = WebcamProcessing.ScanColor(webcam, node.position, node.radius);

            colors.Add(color);
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
            Handles.color = outfit.gizmosColor;

            foreach (OutfitNode node in outfit.nodes)
            {
                Vector3 position = node.position * height;
                float radius = node.radius * height;

                Handles.DrawWireArc(transform.position + position, Vector3.forward, Vector3.up, 360, radius);
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