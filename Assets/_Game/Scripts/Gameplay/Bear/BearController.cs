using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private BearGraphics graphics;

    public event Action onFinishPlaying;

    #region Init

    public void Init()
    {
        graphics.Init();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Public Methods

    public Task SetData(Outfit outfit, List<Color> colors) => graphics.SetData(outfit, colors);

    public void Play() => graphics.Play();

    public void AnimFinish() => onFinishPlaying?.Invoke();

    #endregion

}