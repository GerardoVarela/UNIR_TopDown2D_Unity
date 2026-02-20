using System;
using UnityEngine;

public enum UIClipType
{
    Undefined,
    ButtonClick,
    OpenPopUp,
    ClosePopUp
}

public enum SFXType
{
    Undefined,
    PlayerAttack1,
    VampireAttack1,
    OrcAttack1,
    OpenDoor,
    LifeUp,
    ShadowTotem,
    PickedUpCoin,
}

public enum MusicType
{
    Undefined,
    TitleTheme,
    WelcomeToTown,
    TheIcyCave,
    IntoTheDungeon,
    DecisiveBattle,
}

[Serializable]
public struct UIClipData
{
    public UIClipType type;
    public AudioClip clip;
}

[Serializable]
public struct SFXClipData
{
    public SFXType type;
    public AudioClip[] clips;
}

[Serializable]
public struct MusicClipData
{
    public MusicType type;
    public AudioClip clip;
}

[Serializable]
public struct LevelMusicData
{
    public Level level;
    public MusicType music;
}


[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Scriptable Objects/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [Header("UI Sounds")]
    [SerializeField] private UIClipData[] uiClipList = default;
    [SerializeField] private SFXClipData[] sfxClipList = default;
    [SerializeField] private MusicClipData[] musicClipList = default;

    [Header("Level Music Mapping")]
    [SerializeField] private LevelMusicData[] levelMusicList = default;

    public AudioClip GetUIClip(UIClipType type)
    {
        if (type == UIClipType.Undefined)
            return null;

        foreach (UIClipData data in uiClipList)
        {
            if (data.type == type)
                return data.clip;
        }

        Debug.LogWarning($"UI Clip not found for type: {type}");
        return null;
    }

    public AudioClip GetRandomSFXClip(SFXType type)
    {
        if (type == SFXType.Undefined)
            return null;

        foreach (SFXClipData data in sfxClipList)
        {
            if (data.type == type)
            {
                if (data.clips == null || data.clips.Length == 0)
                    return null;

                int randomIndex = UnityEngine.Random.Range(0, data.clips.Length);
                return data.clips[randomIndex];
            }
        }

        Debug.LogWarning($"SFX Clip not found for type: {type}");
        return null;
    }

    public AudioClip GetMusicClip(MusicType type)
    {
        if (type == MusicType.Undefined)
            return null;

        foreach (MusicClipData data in musicClipList)
        {
            if (data.type == type)
                return data.clip;
        }

        Debug.LogWarning($"Music Clip not found for type: {type}");
        return null;
    }

    public MusicType GetMusicForLevel(Level level)
    {
        foreach (LevelMusicData data in levelMusicList)
        {
            if (data.level == level)
                return data.music;
        }

        Debug.LogWarning($"Music not mapped for level: {level}");
        return MusicType.Undefined;
    }

}
