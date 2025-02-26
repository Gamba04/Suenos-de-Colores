using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WalkingBearsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WalkingBear prefab;

    [Header("Settings")]
    [SerializeField]
    private uint poolSize = 10;
    [SerializeField]
    private Vector3 spawnPoint;

    private readonly Queue<WalkingBear> bears = new Queue<WalkingBear>();

    #region Init

    public void Init()
    {
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            WalkingBear bear = Instantiate(prefab, transform);
            bear.name = $"Bear {i}";

            bear.Init();

            bears.Enqueue(bear);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public async void SpawnBear(SkinnedMeshRenderer data)
    {
        WalkingBear bear = GetNextBear();

        if (bear.IsActive)
        {
            await bear.Despawn();
        }

        bear.Spawn(spawnPoint, data);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private WalkingBear GetNextBear()
    {
        WalkingBear bear = bears.Dequeue();
        bears.Enqueue(bear);

        bear.transform.SetAsLastSibling();

        return bear;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        const float radius = 0.2f;

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(spawnPoint, radius);

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.LowerCenter;
        style.normal.textColor = Color.black;

        Handles.Label(spawnPoint + Vector3.down * radius, "Spawn", style);
    }

#endif

    #endregion

}