using System.Threading.Tasks;
using UnityEngine;

public class WalkingBear : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private ParticleSystem smoke;

    [Header("Settings")]
    [SerializeField]
    private float smokeDelay = 0.5f;

    public bool IsActive
    {
        get => gameObject.activeInHierarchy;
        private set => gameObject.SetActive(value);
    }

    #region Init

    public void Init()
    {
        IsActive = false;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Spawn(Vector3 position)
    {
        IsActive = true;

        transform.position = position;

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