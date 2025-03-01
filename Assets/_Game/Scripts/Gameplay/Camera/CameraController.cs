using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region Custom Data

    private struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;

        public TransformData(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
    }

    #endregion

    [Header("Components")]
    [SerializeField]
    private Animator animator;

    [Header("Settings")]
    [SerializeField]
    private MultiTransition<Vector3, Quaternion> cameraTransition;

    private TransformData origin;

    #region Init

    public void Init()
    {
        SetAnimator(false);

        origin = new TransformData(transform);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        cameraTransition.UpdateTransition(transform.SetPositionAndRotation);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Play()
    {
        SetAnimator(true);
    }

    public async Task Stop()
    {
        SetAnimator(false);

        TaskCompletionSource<bool> delay = new TaskCompletionSource<bool>();

        cameraTransition.SetValues(transform.position, transform.rotation);
        cameraTransition.StartTransition(origin.position, origin.rotation, () => delay.SetResult(true));

        await delay.Task;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void SetAnimator(bool enabled)
    {
        animator.enabled = enabled;

        if (enabled) animator.Rebind();
    }

    #endregion

}