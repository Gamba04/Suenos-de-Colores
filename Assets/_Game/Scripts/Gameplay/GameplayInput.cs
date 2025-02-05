using System;
using System.Collections.Generic;
using UnityEngine;

public enum BearType
{
    Bear_A,
    Bear_B,
    Bear_C,
}

public class GameplayInput : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    private class Keybind
    {
        [SerializeField, HideInInspector] private string name;

        public KeyCode key;

        public void SetName(int index)
        {
            BearType bear = (BearType)index;

            name = $"{bear} ({key})";
        }
    }

    #endregion

    [SerializeField]
    private List<Keybind> keybinds;

    public event Action<BearType> onInput;

    #region Update

    private void Update()
    {
        for (int i = 0; i < keybinds.Count; i++)
        {
            if (Input.GetKeyDown(keybinds[i].key))
            {
                onInput?.Invoke((BearType)i);
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        keybinds.Resize(typeof(BearType));
        keybinds.ForEach((keybind, index) => keybind.SetName(index));
    }

#endif

    #endregion

}