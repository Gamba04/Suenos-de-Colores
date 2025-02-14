using UnityEngine;

public class InactivityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private Animator animator;

    [Header("Settings")]
    [SerializeField]
    private float inactivityCooldown = 10;

    private readonly int inactiveID = Animator.StringToHash("Inactive");

    private readonly Timer.CancelRequest inactivityCancel = new Timer.CancelRequest();

    #region Init

    public void Init()
    {
        StartCooldown();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void OnStartInteraction()
    {
        StopInactivity();
    }

    public void OnFinishInteraction()
    {
        StartCooldown();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Inactivity

    private void StartCooldown()
    {
        Timer.CallOnDelay(StartInactivity, inactivityCooldown, inactivityCancel, "Inactivity cooldown");
    }

    private void StartInactivity()
    {
        animator.SetBool(inactiveID, true);
    }

    private void StopInactivity()
    {
        inactivityCancel.Cancel();

        animator.SetBool(inactiveID, false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        inactivityCooldown = Mathf.Max(inactivityCooldown, 0);
    }

#endif

    #endregion

}