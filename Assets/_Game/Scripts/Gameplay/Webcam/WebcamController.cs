using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class WebcamController : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class OutfitData
    {
        [SerializeField, HideInInspector] private string name;

        public WebcamDataAsset data;

        public void SetName(Outfit outfit)
        {
            name = outfit.ToString();
        }
    }

    #region Custom Editor

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(OutfitData))]
    private class OutfitDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty data = property.FindPropertyRelative(nameof(OutfitData.data));

            EditorGUI.PropertyField(position, data, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

#endif

    #endregion

    #endregion

    [SerializeField]
    private List<OutfitData> outfits = new List<OutfitData>();
    [ReadOnly, SerializeField]
    private List<Color> colors;

    private WebCamTexture webcam;

    #region Init

    public async Task Init()
    {
        webcam = new WebCamTexture();

        await GenerateFrame();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public async Task<List<Color>> GetOutfitColors(Outfit outfit)
    {
        List<WebcamDataAsset.OutfitNode> nodes = outfits[(int)outfit].data.Nodes;
        colors = new List<Color>(nodes.Count);

        Texture2D picture = await TakePicture();

        await Task.Yield();

        foreach (WebcamDataAsset.OutfitNode node in nodes)
        {
            Color color = await WebcamProcessing.ScanColor(picture, node.position, node.size);

            colors.Add(color);
        }

        return colors;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Webcam

    private async Task<Texture2D> TakePicture()
    {
        await GenerateFrame();

        Texture2D texture = new Texture2D(webcam.width, webcam.height);

        Color32[] pixels = webcam.GetPixels32();

        await Task.Yield();

        texture.SetPixels32(pixels);

        await Task.Yield();

        texture.Apply();

        return texture;
    }

    private async Task GenerateFrame()
    {
        webcam.Play();

        do await Task.Yield();
        while (!webcam.didUpdateThisFrame);

        webcam.Pause();
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