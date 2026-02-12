using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager.cs error: The instance is Null");

            return _instance;
        }
    }

    // Current session data
    private string currentSessionName;
    private PlayerSesionData currentSessionData;

    public string CurrentSessionName => currentSessionName;
    public PlayerSesionData CurrentSessionData => currentSessionData;

    private void Awake()
    {
        // Initialize the singleton instance
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        CheckingLevel();
    }

    private void CheckingLevel()
    {
        // Check what level is currently loaded and set the current level in the GameManager
        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] levelsGetted = System.Enum.GetNames(typeof(Level));
        string[] levels = levelsGetted.Select(level => char.ToUpper(level[0]) + level.Substring(1).ToLower()).ToArray();

        bool isALevel = levels.Any(level => level == currentSceneName);

        if (isALevel)
        {
            Level currentLevel = (Level)System.Enum.Parse(typeof(Level), currentSceneName.ToUpper());
            // Debug.Log($"GameManager: Current level detected: {currentLevel}");

            // If no active session, try to load the last game played (useful for editor testing)
            if (!HasActiveSession())
            {
                // Debug.Log("GameManager: No active session detected, attempting to load last played game");
                TryLoadLastGameForLevel(currentLevel);
            }
            else
            {
                // Update current session level if a session is already active
                currentSessionData.level = currentLevel;
                // Debug.Log($"GameManager: Updated active session '{currentSessionName}' to level {currentLevel}");
            }
        }
        else
        {
            // Debug.Log("Current scene is not a level: " + currentSceneName);
        }
    }

    private void TryLoadLastGameForLevel(Level currentLevel)
    {
        // Get the last game played
        string lastGameName = SaveManager.GetLastGamePlayed();

        if (string.IsNullOrWhiteSpace(lastGameName))
        {
            // Debug.Log("GameManager: No last game found, creating temporary development session");
            CreateDevelopmentSession(currentLevel);
            return;
        }

        // Try to load the last game session
        PlayerSesionData lastGameData = SaveManager.LoadSession(lastGameName);

        if (lastGameData == null)
        {
            // Debug.LogWarning($"GameManager: Could not load last game '{lastGameName}', creating temporary session");
            CreateDevelopmentSession(currentLevel);
            return;
        }

        // Set this as the active session
        currentSessionName = lastGameName;
        currentSessionData = lastGameData;

        // Check if the saved level matches the current level
        if (lastGameData.level == currentLevel)
        {
            // Debug.Log($"GameManager: Loaded last game '{lastGameName}' - Level matches, using saved data (Position: {lastGameData.playerPosition}, Health: {lastGameData.health}, Coins: {lastGameData.coins})");
        }
        else
        {
            // Debug.Log($"GameManager: Loaded last game '{lastGameName}' but level mismatch (Saved: {lastGameData.level}, Current: {currentLevel}). Session active but position will reset.");
            // Update the level but reset position since we're in a different level
            currentSessionData.level = currentLevel;
            currentSessionData.playerPosition = Vector2.zero;
        }
    }

    private void CreateDevelopmentSession(Level currentLevel)
    {
        currentSessionName = "DevSession";
        currentSessionData = new PlayerSesionData();
        currentSessionData.level = currentLevel;
        currentSessionData.playerPosition = Vector2.zero;

        // Debug.Log($"GameManager: Created development session for level {currentLevel} (not saved to disk)");
    }

    private void Start()
    {
        print("Starting GameManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGameSession(string sessionName)
    {
        if (string.IsNullOrWhiteSpace(sessionName))
        {
            // Debug.LogError("GameManager: Cannot load session - name is null or empty");
            return;
        }

        PlayerSesionData sessionData = SaveManager.LoadSession(sessionName);

        if (sessionData == null)
        {
            // Debug.LogError($"GameManager: Failed to load session '{sessionName}'");
            return;
        }

        currentSessionName = sessionName;
        currentSessionData = sessionData;

        // Debug.Log($"GameManager: Loaded session '{sessionName}' - Level: {sessionData.level}, Health: {sessionData.health}, Coins: {sessionData.coins}");

        // Load the level from the session
        LoadLevel(sessionData.level);
    }

    public void StartNewGameSession(string sessionName)
    {
        if (string.IsNullOrWhiteSpace(sessionName))
        {
            // Debug.LogError("GameManager: Cannot create new session - name is null or empty");
            return;
        }

        // Check if session already exists
        if (SaveManager.SessionExists(sessionName))
        {
            // Debug.LogWarning($"GameManager: Session '{sessionName}' already exists. Loading existing session instead.");
            LoadGameSession(sessionName);
            return;
        }

        // Create new session data
        currentSessionData = SaveManager.CreateNewGameData(sessionName);
        currentSessionName = sessionName;

        // Debug.Log($"GameManager: Created new session '{sessionName}'");

        // Load the initial level
        LoadLevel(currentSessionData.level);
    }

    public void SaveCurrentSession()
    {
        if (string.IsNullOrWhiteSpace(currentSessionName))
        {
            // Debug.LogWarning("GameManager: No active session to save");
            return;
        }

        if (currentSessionData == null)
        {
            // Debug.LogError("GameManager: Current session data is null");
            return;
        }

        SaveManager.SaveGame(currentSessionName, currentSessionData);
        // Debug.Log($"GameManager: Saved current session '{currentSessionName}'");
    }

    public void UpdatePlayerPosition(Vector2 position)
    {
        if (currentSessionData != null)
        {
            currentSessionData.playerPosition = position;
            // Debug.Log($"GameManager: Updated player position to {position}");
        }
        else
        {
            // Debug.LogWarning("GameManager: Cannot update position - no active session");
        }
    }

    public void UpdatePlayerHealth(int health) // ! Doesn't have references
    {
        if (currentSessionData != null)
        {
            currentSessionData.health = health;
            // Debug.Log($"GameManager: Updated player health to {health}");
        }
        else
        {
            // Debug.LogWarning("GameManager: Cannot update health - no active session");
        }
    }

    public void UpdatePlayerCoins(int coins) // ! Doesn't have references
    {
        if (currentSessionData != null)
        {
            currentSessionData.coins = coins;
            // Debug.Log($"GameManager: Updated player coins to {coins}");
        }
        else
        {
            // Debug.LogWarning("GameManager: Cannot update coins - no active session");
        }
    }

    public void SaveCheckpoint(Vector2 checkpointPosition)
    {
        if (currentSessionData == null)
        {
            // Debug.LogWarning("GameManager: Cannot save checkpoint - no active session");
            return;
        }

        UpdatePlayerPosition(checkpointPosition);
        SaveCurrentSession();
        // Debug.Log($"GameManager: Checkpoint saved at {checkpointPosition}");
    }

    public void NextLevel(Level nextLevel)
    {
        if (currentSessionData == null)
        {
            // Debug.LogWarning("GameManager: Cannot change level - no active session");
            return;
        }

        // Update the level in session data
        currentSessionData.level = nextLevel;

        // Reset player position to default spawn (Vector2.zero indicates no saved position)
        currentSessionData.playerPosition = Vector2.zero;

        // Save the session with new level and reset position
        SaveCurrentSession();

        // Debug.Log($"GameManager: Transitioning to next level '{nextLevel}' - Position reset for new level spawn");

        // Load the new level
        LoadLevel(nextLevel);
    }

    private void LoadLevel(Level level)
    {
        Time.timeScale = 1f;
        string levelName = level.ToString();
        string convertedLevelName = char.ToUpper(levelName[0]) + levelName.Substring(1).ToLower();
        SceneManager.LoadScene(convertedLevelName);
        // Debug.Log($"GameManager: Loading level '{convertedLevelName}'");
    }

    public bool HasActiveSession()
    {
        return !string.IsNullOrWhiteSpace(currentSessionName) && currentSessionData != null;
    }
}
