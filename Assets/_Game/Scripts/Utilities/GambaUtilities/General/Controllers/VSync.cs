using UnityEngine;

public class VSync : MonoBehaviour
{
    [SerializeField]
    private bool vSyncEnabled;
    [SerializeField]
    private int target;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = vSyncEnabled ? target : -1;
    }
}