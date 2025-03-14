using System.Collections.Generic;
using UnityEngine;

public class AnimatorSFXTrigger : MonoBehaviour
{
    [SerializeField]
    private List<SFXTag> sfx;

    public void PlaySFX(int index)
    {
        SFXTag sfx = this.sfx[index];

        SFXPlayer.PlaySFX(sfx);
    }
}