using System.Threading.Tasks;
using UnityEngine;

public class WalkingBear : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private new SkinnedMeshRenderer renderer;
    [SerializeField]
    private ParticleSystem smoke;

    [Header("Settings")]
    [SerializeField]
    private float smokeDelay = 0.5f;

    private MaterialPropertyBlock properties;

    public bool IsActive
    {
        get => gameObject.activeInHierarchy;
        private set => gameObject.SetActive(value);
    }

    #region Init

    public void Init()
    {
        IsActive = false;

        properties = new MaterialPropertyBlock();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Spawn(Vector3 position, SkinnedMeshRenderer data)
    {
        IsActive = true;

        transform.position = position;

        renderer.sharedMesh = data.sharedMesh;

        data.GetPropertyBlock(properties);
        renderer.SetPropertyBlock(properties);

        // Start walking
    }

    public async Task Despawn()
    {
        smoke.Play();

        float duration = smoke.main.startLifetime.constantMax;

        await Delay(smokeDelay);

        IsActive = false;

        await Delay(duration - smokeDelay);

        static Task Delay(float delay) => Task.Delay((int)(delay * 1000));
    }

    #endregion

}