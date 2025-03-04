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
        public KeyCode altKey;

        public bool IsKeyDown => Input.GetKeyDown(key) || Input.GetKeyDown(altKey);

        public void SetName(Outfit outfit)
        {
            name = outfit.ToString();
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
            if (keybinds[i].IsKeyDown)
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
        keybinds.ForEach((keybind, index) => keybind.SetName((Outfit)index));
    }

#endif

    #endregion

}