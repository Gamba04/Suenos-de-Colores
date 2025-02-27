using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class WalkingBearsController : MonoBehaviour
{

    #region Custom Data

    [Serializable]
    public struct WalkingArea
    {
        public Vector3 center;
        public Vector3 size;

        public Vector3 GetRandomPosition()
        {
            Vector3 position = new Vector3()
            {
                x = GetRandomValue() * size.x,
                z = GetRandomValue() * size.z
            };

            return center + position;

            static float GetRandomValue() => UnityEngine.Random.value - 0.5f;
        }
    }

    #endregion

    [Header("Components")]
    [SerializeField]
    private WalkingBear prefab;

    [Header("Settings")]
    [SerializeField]
    private uint poolSize = 10;
    [SerializeField]
    private Vector3 spawnPoint;

    [Space]
    [SerializeField]
    private WalkingArea walkingArea;

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

            bear.Init(walkingArea);

            bears.Enqueue(bear);
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void SpawnBear(SkinnedMeshRenderer data)
    {
        WalkingBear bear = GetNextBear();

        StartCoroutine(Spawn());

        IEnumerator Spawn()
        {
            if (bear.IsActive)
            {
                yield return StartCoroutine(bear.Despawn());
            }

            bear.Spawn(spawnPoint, data);
        }
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
        DrawSpawnPoint();
        DrawWalkingArea();
    }

    private void DrawSpawnPoint()
    {
        const float radius = 0.2f;

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(spawnPoint, radius);

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.LowerCenter;
        style.normal.textColor = Color.black;

        Handles.Label(spawnPoint + Vector3.down * radius, "Spawn", style);
    }

    private void DrawWalkingArea()
    {
        Color color = Color.black;
        color.a = 0.1f;

        Gizmos.color = color;

        Vector3 offset = Vector3.up * 0.015f;
        Vector3 center = walkingArea.center + offset;

        Gizmos.DrawCube(center, walkingArea.size);
        Gizmos.DrawWireCube(center, walkingArea.size);
    }

#endif

    #endregion

}