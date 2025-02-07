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
        public List<OutfitNode> nodes;

        public void SetName(OutfitTag outfit)
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
        public float radius;

        public void SetName(int index)
        {
            name = $"Node {index}";
        }
    }

    #endregion

    [SerializeField]
    private List<OutfitData> outfits;

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

    public Color GetCurrentColor()
    {
        return WebcamProcessing.GetColor(webcam, default);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        RectTransform root = transform as RectTransform;

        float scaling = root.rect.height * root.lossyScale.y / 2;

        foreach (OutfitData outfit in outfits)
        {
            Handles.color = outfit.gizmosColor;

            foreach (OutfitNode node in outfit.nodes)
            {
                Vector3 position = node.position * scaling;
                float radius = node.radius * scaling;

                Handles.DrawWireArc(transform.position + position, Vector3.forward, Vector3.up, 360, radius);
            }
        }
    }

    private void OnValidate()
    {
        outfits.Resize(typeof(OutfitTag));
        outfits.ForEach((outfit, index) => outfit.SetName((OutfitTag)index));
    }

#endif

    #endregion

}