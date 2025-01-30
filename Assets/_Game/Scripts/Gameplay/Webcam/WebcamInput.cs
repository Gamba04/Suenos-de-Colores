using System;
using UnityEngine;

public class WebcamInput : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private KeyCode keybind = KeyCode.Space;

    private bool detecting;

    public Action onTrigger;

    #region Update

    private void Update()
    {
        UpdateInput();   
    }

    private void UpdateInput()
    {
        if (!detecting) return;

        if (Input.GetKeyDown(keybind))
        {
            Trigger();
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SetInput(bool enabled)
    {
        detecting = enabled;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void Trigger()
    {
        onTrigger?.Invoke();
    }

    #endregion

}