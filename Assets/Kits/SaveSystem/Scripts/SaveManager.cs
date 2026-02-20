using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    private static string dataPath = Application.persistentDataPath + "/PlayersData.save";
    private static string lastGamePath = Application.persistentDataPath + "/LastGame.save";

    public static GameData LoadAllGames()
    {
        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("SaveManager: No save file found at " + dataPath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(dataPath);
            Debug.Log($"SaveManager: Loading data from file. JSON content: {json}");

            GameData data = JsonUtility.FromJson<GameData>(json);

            if (data == null)
            {
                Debug.LogWarning("SaveManager: Failed to deserialize save file");
                return null;
            }

            if (data.sessions == null || data.sessions.Count == 0)
            {
                Debug.LogWarning("SaveManager: Save file has no sessions");
                return data;
            }

            Debug.Log($"SaveManager: Successfully loaded {data.sessions.Count} game session(s)");

            // Log each session for debugging
            foreach (var session in data.sessions)
            {
                Debug.Log($"  - Session: '{session.sessionName}' | Level: {session.sessionData?.level} | Health: {session.sessionData?.health} | Coins: {session.sessionData?.coins}");
            }

            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Error loading games - {e.Message}\n{e.StackTrace}");
            return null;
        }
    }

    public static string GetLastGamePlayed()
    {
        if (!File.Exists(lastGamePath))
        {
            Debug.LogWarning("SaveManager: No last game record found");
            return null;
        }

        try
        {
            string lastGameName = File.ReadAllText(lastGamePath);
            Debug.Log($"SaveManager: Last game played was '{lastGameName}'");
            return lastGameName;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Error reading last game - {e.Message}");
            return null;
        }
    }

    public static PlayerSesionData LoadSession(string sessionName)
    {
        GameData allData = LoadAllGames();

        if (allData == null)
        {
            Debug.LogWarning($"SaveManager: Cannot load session '{sessionName}' - no save data exists");
            return null;
        }

        PlayerSesionData sessionData = allData.GetSession(sessionName);

        if (sessionData != null)
        {
            Debug.Log($"SaveManager: Loaded session '{sessionName}' - Level: {sessionData.level}, Health: {sessionData.health}, Coins: {sessionData.coins}");
            return sessionData;
        }
        else
        {
            Debug.LogWarning($"SaveManager: Session '{sessionName}' not found in save file");
            return null;
        }
    }

    public static void SaveGame(string sessionName, PlayerSesionData sessionData)
    {
        if (string.IsNullOrWhiteSpace(sessionName))
        {
            Debug.LogError("SaveManager: Cannot save game - session name is null or empty");
            return;
        }

        if (sessionData == null)
        {
            Debug.LogError("SaveManager: Cannot save game - session data is null");
            return;
        }

        Debug.Log($"SaveManager: Attempting to save session '{sessionName}' - Level: {sessionData.level}, Health: {sessionData.health}, Coins: {sessionData.coins}");

        // Load existing data or create new
        GameData allData = LoadAllGames();

        if (allData == null)
        {
            Debug.Log("SaveManager: No existing save file, creating new GameData");
            allData = new GameData();
        }

        // Add or update the session
        allData.SetSession(sessionName, sessionData);

        try
        {
            // Save the updated data
            string json = JsonUtility.ToJson(allData, true);
            Debug.Log($"SaveManager: Saving JSON to {dataPath}:\n{json}");
            File.WriteAllText(dataPath, json);

            // Update last game played
            File.WriteAllText(lastGamePath, sessionName);

            Debug.Log($"SaveManager: Successfully saved session '{sessionName}' to disk");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SaveManager: Error saving game - {e.Message}\n{e.StackTrace}");
        }
    }

    public static PlayerSesionData CreateNewGameData(string sessionName)
    {
        if (string.IsNullOrWhiteSpace(sessionName))
        {
            Debug.LogError("SaveManager: Cannot create new game - session name is null or empty");
            return null;
        }

        PlayerSesionData newData = new PlayerSesionData();
        Debug.Log($"SaveManager: Created new game data for session '{sessionName}' - Level: {newData.level}, Health: {newData.health}, Coins: {newData.coins}");

        // Save the new game immediately
        SaveGame(sessionName, newData);

        return newData;
    }

    public static bool SessionExists(string sessionName)
    {
        GameData allData = LoadAllGames();

        if (allData == null)
        {
            Debug.Log($"SaveManager: SessionExists('{sessionName}') - No save file exists, returning false");
            return false;
        }

        bool exists = allData.HasSession(sessionName);
        Debug.Log($"SaveManager: SessionExists('{sessionName}') - {exists}");
        return exists;
    }

    public static bool DeleteSession(string sessionName)
    {
        GameData allData = LoadAllGames();

        if (allData == null)
        {
            Debug.LogWarning($"SaveManager: Cannot delete session '{sessionName}' - no save data exists");
            return false;
        }

        if (allData.RemoveSession(sessionName))
        {
            try
            {
                string json = JsonUtility.ToJson(allData, true);
                File.WriteAllText(dataPath, json);
                Debug.Log($"SaveManager: Deleted session '{sessionName}'");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"SaveManager: Error deleting session - {e.Message}");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"SaveManager: Session '{sessionName}' not found for deletion");
            return false;
        }
    }

    public static List<string> GetAllSessionNames()
    {
        GameData allData = LoadAllGames();

        if (allData == null)
            return new List<string>();

        return allData.GetAllSessionNames();
    }
}
