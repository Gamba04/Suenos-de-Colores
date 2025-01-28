using System;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Image fade;

    [Header("Settings")]
    [SerializeField]
    private bool startWithFade = true;
    [SerializeField]
    private bool enableInteractionsAfterFade = true;

    [Space]
    [SerializeField]
    private Color color = Color.black;
    [SerializeField]
    private TransitionColor transition;

    public event Action onFinishFade;

    public bool IsOnTransition => transition.IsOnTransition;

    #region Init

    #region Singleton

    private static UIFade instance;

    public static UIFade Instance => GambaFunctions.GetSingleton(ref instance);

    private void Awake()
    {
        GambaFunctions.InitSingleton(ref instance, this);

        Init();
    }

    #endregion

    private void Init()
    {
        if (!startWithFade) return;

        Button.Interactions = false;

        SetFade(true, true);

        Action onFinishFade = null;
        if (enableInteractionsAfterFade) onFinishFade = () => Button.Interactions = true;

        SetFade(false, onFinishFade: onFinishFade);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        transition.UpdateTransition(color => fade.color = color);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static void SetFade(bool value, bool instant = false, Action onFinishFade = null) => Instance.SetFadeInternal(value, instant, onFinishFade);

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Fade

    private void SetFadeInternal(bool value, bool instant, Action onFinishFade)
    {
        // SetActive
        if (value) fade.gameObject.SetActive(true);
        else onFinishFade += () => fade.gameObject.SetActive(false);

        // Fade events
        onFinishFade += this.onFinishFade;

        // Transition
        Color targetColor = value ? color : color.GetAlpha(0);

        if (instant)
        {
            transition.value = targetColor;
            fade.color = targetColor;

            onFinishFade?.Invoke();
        }
        else
        {
            transition.StartTransition(targetColor, onFinishFade);
        }
    }

    #endregion

}