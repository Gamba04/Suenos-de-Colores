using System.Threading.Tasks;
using UnityEngine;

public class InactivityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CameraController cameraController;

    [Header("Settings")]
    [SerializeField]
    private float inactivityCooldown = 10;

    private readonly Timer.CancelRequest inactivityCancel = new Timer.CancelRequest();

    private bool isInactive;

    #region Init

    public void Init()
    {
        cameraController.Init();

        StartCooldown();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public async Task OnStartInteraction()
    {
        await StopInactivity();
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
        isInactive = true;

        cameraController.Play();
    }

    private async Task StopInactivity()
    {
        if (isInactive)
        {
            isInactive = false;

            await cameraController.Stop();
        }
        else
        {
            inactivityCancel.Cancel();
        }
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