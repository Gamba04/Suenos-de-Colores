using System;
using System.Collections.Generic;
using UnityEngine;

public class BearGraphics : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class BearData
    {
        [SerializeField, HideInInspector] private string name;

        public Mesh mesh;

        public void SetName(BearType bear)
        {
            name = bear.ToString();
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
    private List<BearData> bears;

    private readonly int playID = Animator.StringToHash("Play");
    private readonly int animationID = Animator.StringToHash("Animation");

    private readonly int colorID = Shader.PropertyToID("_Color");

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

    public void SetBear(BearType bear)
    {
        BearData data = bears[(int)bear];

        filter.mesh = data.mesh;
    }

    public void SetColor(Color color)
    {
        properties.SetColor(colorID, color);
        renderer.SetPropertyBlock(properties);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        bears.Resize(typeof(BearType));
        bears.ForEach((bear, index) => bear.SetName((BearType)index));
    }

#endif

    #endregion

}