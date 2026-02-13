using UnityEngine;

[System.Serializable]
public class LevelMusicEntry
{
    public Level level;
    public MusicType music;
}


public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [Header("Database")]
    [SerializeField] private SoundDatabase database;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region MUSIC

    public void PlayLevelMusic(Level level, bool loop = true)
    {
        MusicType musicType = database.GetMusicForLevel(level);

        if (musicType == MusicType.Undefined)
            return;

        PlayMusic(musicType, loop);
    }


    public void PlayMusic(MusicType type, bool loop = true)
    {
        AudioClip clip = database.GetMusicClip(type);
        if (clip == null)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    #endregion

    #region UI

    public void PlayUI(UIClipType type)
    {
        AudioClip clip = database.GetUIClip(type);
        if (clip == null)
            return;

        uiSource.PlayOneShot(clip);
    }

    #endregion

    #region SFX

    public void PlaySFX(SFXType type)
    {
        AudioClip clip = database.GetRandomSFXClip(type);
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    // Extra: reproducir SFX en cualquier AudioSource
    public void PlaySFX(AudioSource source, SFXType type)
    {
        AudioClip clip = database.GetRandomSFXClip(type);
        if (clip == null || source == null)
            return;
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    #endregion
}
