using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamPreview : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class OutfitData
    {
        [SerializeField, HideInInspector] private string name;

        public WebcamDataAsset data;
        public Texture2D lineArt;

        public void SetName(Outfit outfit)
        {
            string data = this.data != null ? this.data.name : "None";

            name = $"{outfit} ({data})";
        }
    }

    #endregion

    [Header("Components")]
    [SerializeField]
    private RawImage preview;

    [Header("Data")]
    [SerializeField]
    private List<OutfitData> outfits = new List<OutfitData>();

    [Header("Preview")]
    [SerializeField]
    private Outfit previewOutfit;
    [SerializeField]
    private Color gizmosColor = Color.white;

    #region Editor

#if UNITY_EDITOR

    #region Gizmos

    private void OnDrawGizmos()
    {
        WebcamDataAsset data = outfits[(int)previewOutfit].data;

        RectTransform root = transform as RectTransform;
        float height = root.rect.height * root.lossyScale.y;

        DrawOutfitGizmos(data, height);
    }

    private void DrawOutfitGizmos(WebcamDataAsset data, float height)
    {
        if (data == null) return;

        foreach (WebcamDataAsset.OutfitNode node in data.Nodes)
        {
            Vector3 position = transform.position + (Vector3)node.position * height;
            Vector2 size = Vector2.one * node.size * height;

            Gizmos.color = gizmosColor;
            Gizmos.DrawWireCube(position, size);

            Gizmos.color = gizmosColor.GetAlpha(0.1f);
            Gizmos.DrawCube(position, size);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Inspector

    private void OnValidate()
    {
        UpdateEditorOutfits();
        UpdateEditorLineArt();
    }

    private void UpdateEditorOutfits()
    {
        outfits.Resize(typeof(Outfit));
        outfits.ForEach((outfit, index) => outfit.SetName((Outfit)index));
    }

    private void UpdateEditorLineArt()
    {
        OutfitData outfit = outfits[(int)previewOutfit];

        preview.texture = outfit.lineArt;
    }

    #endregion

#endif

    #endregion

}