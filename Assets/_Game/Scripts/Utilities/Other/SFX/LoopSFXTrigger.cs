using UnityEngine;

public class LoopSFXTrigger : MonoBehaviour
{
    [Space]
    [SerializeField]
    private SFXLoopTag sfx;

    private void Start()
    {
        SFXPlayer.SetSFXLoop(sfx, transform.position);
        SFXPlayer.SetSFXLoop(sfx, true);
    }
}