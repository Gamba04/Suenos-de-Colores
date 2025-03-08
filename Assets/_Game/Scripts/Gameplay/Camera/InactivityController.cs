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
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void StartCooldown()
    {
        Timer.CallOnDelay(StartInactivity, inactivityCooldown, inactivityCancel, "Inactivity cooldown");

        void StartInactivity()
        {
            isInactive = true;

            cameraController.Play();
        }
    }

    public async Task StopInactivity()
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