using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSessionEntry
{
    public string sessionName;
    public PlayerSesionData sessionData;

    public SerializableSessionEntry() { }

    public SerializableSessionEntry(string name, PlayerSesionData data)
    {
        sessionName = name;
        sessionData = data;
    }
}

[System.Serializable]
public class GameData
{
    // Unity's JsonUtility cannot serialize Dictionary, so we use a List instead
    public List<SerializableSessionEntry> sessions = new List<SerializableSessionEntry>();

    public Dictionary<string, PlayerSesionData> sesionsData
    {
        get
        {
            Dictionary<string, PlayerSesionData> dict = new Dictionary<string, PlayerSesionData>();

            if (sessions != null)
            {
                foreach (var entry in sessions)
                {
                    if (!string.IsNullOrEmpty(entry.sessionName) && entry.sessionData != null)
                    {
                        dict[entry.sessionName] = entry.sessionData;
                    }
                }
            }

            return dict;
        }
        set
        {
            sessions = new List<SerializableSessionEntry>();

            if (value != null)
            {
                foreach (var kvp in value)
                {
                    sessions.Add(new SerializableSessionEntry(kvp.Key, kvp.Value));
                }
            }
        }
    }

    public GameData()
    {
        sessions = new List<SerializableSessionEntry>();
    }

    public GameData(Dictionary<string, PlayerSesionData> pSesionsData)
    {
        sessions = new List<SerializableSessionEntry>();

        if (pSesionsData != null)
        {
            foreach (var kvp in pSesionsData)
            {
                sessions.Add(new SerializableSessionEntry(kvp.Key, kvp.Value));
            }
        }
    }

    public PlayerSesionData GetSession(string sessionName)
    {
        if (sessions == null) return null;

        foreach (var entry in sessions)
        {
            if (entry.sessionName == sessionName)
            {
                return entry.sessionData;
            }
        }

        return null;
    }

    public bool HasSession(string sessionName)
    {
        if (sessions == null) return false;

        foreach (var entry in sessions)
        {
            if (entry.sessionName == sessionName)
            {
                return true;
            }
        }

        return false;
    }

    public void SetSession(string sessionName, PlayerSesionData sessionData)
    {
        if (sessions == null)
        {
            sessions = new List<SerializableSessionEntry>();
        }

        // Try to find and update existing session
        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].sessionName == sessionName)
            {
                sessions[i].sessionData = sessionData;
                Debug.Log($"GameData: Updated session '{sessionName}'");
                return;
            }
        }

        // If not found, add new session
        sessions.Add(new SerializableSessionEntry(sessionName, sessionData));
        Debug.Log($"GameData: Added new session '{sessionName}'");
    }

    public bool RemoveSession(string sessionName)
    {
        if (sessions == null) return false;

        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].sessionName == sessionName)
            {
                sessions.RemoveAt(i);
                Debug.Log($"GameData: Removed session '{sessionName}'");
                return true;
            }
        }

        return false;
    }

    public List<string> GetAllSessionNames()
    {
        List<string> names = new List<string>();

        if (sessions != null)
        {
            foreach (var entry in sessions)
            {
                if (!string.IsNullOrEmpty(entry.sessionName))
                {
                    names.Add(entry.sessionName);
                }
            }
        }

        return names;
    }
}