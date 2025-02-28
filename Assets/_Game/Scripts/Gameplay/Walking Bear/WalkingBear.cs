using System.Collections;
using UnityEngine;

public class WalkingBear : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private new BoxCollider collider;
    [SerializeField]
    private new SkinnedMeshRenderer renderer;
    [SerializeField]
    private ParticleSystem smoke;

    [Header("Settings")]
    [SerializeField]
    private float smokeDelay = 0.5f;
    [SerializeField]
    private float walkSpeed = 1;
    [SerializeField]
    private float idleTime = 5;

    [Space]
    [SerializeField]
    private TransitionQuaternion lookTransition;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask detectionLayer;
    [SerializeField]
    private float detectionRange = 1;

    private readonly int walkingID = Animator.StringToHash("Walking");

    private MaterialPropertyBlock properties;

    private WalkingBearsController.WalkingArea walkingArea;
    private Coroutine walkingLoop;

    #region State

    public bool IsActive
    {
        get => gameObject.activeInHierarchy;
        private set => gameObject.SetActive(value);
    }

    public bool IsVisible
    {
        get => root.activeInHierarchy;
        private set => root.SetActive(value);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Init

    public void Init(WalkingBearsController.WalkingArea walkingArea)
    {
        this.walkingArea = walkingArea;

        properties = new MaterialPropertyBlock();

        IsActive = false;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        lookTransition.UpdateTransition(OnLookTransition);
    }

    private void OnLookTransition(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Spawn(Vector3 position, SkinnedMeshRenderer data)
    {
        IsActive = true;
        IsVisible = true;

        transform.position = position;

        renderer.sharedMesh = data.sharedMesh;

        data.GetPropertyBlock(properties);
        renderer.SetPropertyBlock(properties);

        walkingLoop = StartCoroutine(StartWalking());
    }

    public IEnumerator Despawn()
    {
        StopWalking();

        smoke.Play();

        float duration = smoke.main.startLifetime.constantMax;

        yield return new WaitForSeconds(smokeDelay);

        IsVisible = false;

        yield return new WaitForSeconds(duration - smokeDelay);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Walking

    private IEnumerator StartWalking()
    {
        while (true)
        {
            Vector3 target = GetRandomPosition();

            yield return Walk(target);

            yield return new WaitForSeconds(idleTime);
        }
    }

    private IEnumerator Walk(Vector3 target)
    {
        SetWalking(true);

        Vector3 origin = transform.position;
        Vector3 route = target - origin;

        float duration = route.magnitude / walkSpeed;
        float progress = 0;

        LookTowards(route.normalized);

        while (progress < 1)
        {
            if (CheckCollision(route.normalized)) break;

            progress += Time.deltaTime / duration;
            progress = Mathf.Min(progress, 1);

            transform.position = Vector3.Lerp(origin, target, progress);

            yield return null;
        }

        SetWalking(false);
        LookTowards(GetRandomDirection());
    }

    private void SetWalking(bool value)
    {
        animator.SetBool(walkingID, value);
    }

    private void StopWalking()
    {
        StopCoroutine(walkingLoop);
        SetWalking(false);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private Vector3 GetRandomPosition()
    {
        return walkingArea.GetRandomPosition();
    }

    private Vector3 GetRandomDirection()
    {
        float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

    private void LookTowards(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        lookTransition.StartTransition(lookRotation);
    }

    private bool CheckCollision(Vector3 direction)
    {
        Vector3 position = transform.TransformPoint(collider.center);

        return Physics.Raycast(position, direction, detectionRange, detectionLayer);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        ClampToZero(ref smokeDelay);
        ClampToZero(ref walkSpeed);
        ClampToZero(ref idleTime);
        ClampToZero(ref detectionRange);

        static void ClampToZero(ref float value) => value = Mathf.Max(value, 0);
    }

#endif

    #endregion

}