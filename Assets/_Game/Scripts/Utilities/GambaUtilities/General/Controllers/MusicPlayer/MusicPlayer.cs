using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [Space]
    [SerializeField]
    private MusicPlaylistAsset playlist;
    [SerializeField]
    private AudioMixerGroup mix;

    [Header("Settings")]
    [SerializeField]
    private bool autoPlay = true;
    [SerializeField]
    private bool loop = true;
    [SerializeField]
    private bool shuffle;
    [SerializeField]
    [Range(0, 1)]
    private float volume = 1;

    private AudioSource source;

    private int position = -1;

    private static float? persistentVolume;

    #region Properties

    public static bool AutoPlay { get => Instance.autoPlay; set => Instance.autoPlay = value; }

    public static bool Loop { get => Instance.loop; set => Instance.loop = value; }

    public static bool Shuffle { get => Instance.shuffle; set => Instance.shuffle = value; }

    public static float Volume
    {
        get => Instance.volume;
        set
        {
            Instance.volume = value;
            persistentVolume = value;

            if (Source != null) Source.volume = value;
        }
    }

    public static AudioSource Source => Instance.source;

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Init

    #region Singleton

    private static MusicPlayer instance;

    private static MusicPlayer Instance => GambaFunctions.GetSingleton(ref instance);

    private void Awake()
    {
        GambaFunctions.InitSingleton(ref instance, this);

        Init();
    }

    #endregion

    private void Init()
    {
        InitVolume();
        InitAudioSource();

        if (autoPlay) Play();
    }

    private void InitVolume()
    {
        if (persistentVolume.HasValue)
        {
            volume = persistentVolume.Value;
        }
        else persistentVolume = volume;
    }

    private void InitAudioSource()
    {
        source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0;

        source.volume = volume;
        source.outputAudioMixerGroup = mix;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Update

    private void Update()
    {
        UpdateSong();
    }

    private void UpdateSong()
    {
        if (source.clip == null) return;

        bool songFinished = source.timeSamples == source.clip.samples;

        if (songFinished) PlayNextSong();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Static Methods

    public static void Play()
    {
        if (Instance.position < 0) PlayNext();
        else Instance.Resume();
    }

    public static void Pause()
    {
        Instance.StopPlaying(false);
    }

    public static void Stop()
    {
        Instance.StopPlaying(true);
    }

    public static void Restart()
    {
        Stop();
        Play();
    }

    public static void RestartPlaylist()
    {
        Instance.position = -1;

        Restart();
    }

    public static void PlayNext()
    {
        Instance.PlayNextSong();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Playing

    private void PlayNextSong()
    {
        position++;

        AudioClip song = GetSong();

        if (song != null) PlaySong(song);
    }

    private AudioClip GetSong()
    {
        return playlist.GetSong(ref position, loop);
    }

    private void PlaySong(AudioClip song)
    {
        source.clip = song;
        source.Play();
    }

    private void StopPlaying(bool stop)
    {
        if (stop) source.Stop();
        else source.Pause();
    }

    private void Resume()
    {
        if (source.timeSamples == 0) source.Play();
        else source.UnPause();
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        UpdateEditorRuntime();
    }

    private void UpdateEditorRuntime()
    {
        if (!Application.isPlaying) return;

        Volume = volume;
    }

#endif

    #endregion

}