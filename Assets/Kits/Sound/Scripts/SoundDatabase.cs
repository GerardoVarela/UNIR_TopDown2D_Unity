using System;
using UnityEngine;

public enum UIClipType
{
    Undefined,
    ButtonClick,

}

public enum SFXType
{
    Undefined,
    PlayerSttack1,
    PickUpCoin,
    VampireAttack1,
    VampireReceiveDamage,
    VampireDeath,
}

public enum MusicType
{
    Undefined,
    MainMenu,
    Intro,
    Forest,
    Castle,


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
[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Scriptable Objects/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [Header("UI Sounds")]
    private UIClipData[] uiClipList = default;
    private SFXClipData[] sfxClipList = default;
    private MusicClipData[] musicClipList = default;

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
}
