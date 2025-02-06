using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private BearGraphics graphics;

    [Header("Settings")]
    [SerializeField]
    private uint animationsAmount = 1;

    private bool isPlaying;

    public bool IsAvailable => !isPlaying;

    #region Public Methods

    public void Play(BearType bear, Color color)
    {
        isPlaying = true;

        graphics.SetBear(bear);
        graphics.SetColor(color);

        int animation = GetRandomAnimation();

        graphics.Play(animation);
    }

    public void AnimFinish()
    {
        isPlaying = true;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private int GetRandomAnimation()
    {
        return Random.Range(0, (int)animationsAmount);
    }

    #endregion

}