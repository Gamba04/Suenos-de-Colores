using System;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private BearGraphics graphics;

    [Header("Info")]
    [ReadOnly, SerializeField]
    private bool isPlaying;

    public bool IsAvailable => !isPlaying;

    public event Action onFinishAnim;

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
        graphics.Play();
    }

    public void AnimFinish()
    {
        isPlaying = false;

        onFinishAnim?.Invoke();
    }

    #endregion

}