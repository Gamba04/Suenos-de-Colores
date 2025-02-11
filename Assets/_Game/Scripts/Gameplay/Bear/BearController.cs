using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private BearGraphics graphics;

    [Header("Settings")]
    [SerializeField]
    private uint animationsAmount = 1;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private bool isPlaying;

    public bool IsAvailable => !isPlaying;

    #region Init

    public void Init()
    {
        graphics.Init();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public void Play(Outfit outfit, List<Color> colors)
    {
        isPlaying = true;

        graphics.SetData(outfit, colors);

        int animation = GetRandomAnimation();

        graphics.Play(animation);
    }

    public void AnimFinish()
    {
        isPlaying = false;
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