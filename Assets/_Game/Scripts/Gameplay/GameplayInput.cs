using System;
using System.Collections.Generic;
using UnityEngine;

public enum OutfitTag
{
    Look_1,
    Look_2,
    Look_3,
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
            OutfitTag bear = (OutfitTag)index;

            name = $"{bear} ({key})";
        }
    }

    #endregion

    [SerializeField]
    private List<Keybind> keybinds;

    public event Action<OutfitTag> onInput;

    #region Update

    private void Update()
    {
        for (int i = 0; i < keybinds.Count; i++)
        {
            if (Input.GetKeyDown(keybinds[i].key))
            {
                onInput?.Invoke((OutfitTag)i);
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        keybinds.Resize(typeof(OutfitTag));
        keybinds.ForEach((keybind, index) => keybind.SetName(index));
    }

#endif

    #endregion

}