using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = title, menuName = "Gamba/" + title)]
public class WebcamDataScrObj : ScriptableObject
{
    private const string title = "Webcam Data";

    #region Custom Data

    [Serializable]
    public class OutfitNode
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
    private List<OutfitNode> nodes = new List<OutfitNode>();

    public List<OutfitNode> Nodes => nodes;

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        nodes.ForEach((node, index) => node.SetName(index));
    }

#endif

    #endregion

}