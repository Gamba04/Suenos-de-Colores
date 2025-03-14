using UnityEngine;

public class AnimationTimeRandomizer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        int state = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

        animator.Play(state, 0, Random.value);
    }
}