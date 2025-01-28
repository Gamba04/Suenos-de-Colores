﻿using UnityEngine;

namespace Editor
{
    [AddComponentMenu("​Hierarchy Folder")]
    [DisallowMultipleComponent]
    public class HierarchyFolder : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private Color color = Color.white;

        public Color Color => color;
    }
}