using System;
using System.Collections.Generic;
using UnityEngine;

public enum Outfit
{
    Outfit_1,
    Outfit_2,
    Outfit_3,
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
            Outfit bear = (Outfit)index;

            name = $"{bear} ({key})";
        }
    }

    #endregion

    [SerializeField]
    private List<Keybind> keybinds;

    public event Action<Outfit> onInput;

    #region Update

    private void Update()
    {
        for (int i = 0; i < keybinds.Count; i++)
        {
            if (Input.GetKeyDown(keybinds[i].key))
            {
                onInput?.Invoke((Outfit)i);
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        keybinds.Resize(typeof(Outfit));
        keybinds.ForEach((keybind, index) => keybind.SetName(index));
    }

#endif

    #endregion

}