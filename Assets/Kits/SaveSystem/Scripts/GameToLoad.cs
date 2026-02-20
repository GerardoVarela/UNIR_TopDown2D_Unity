using UnityEngine;
using TMPro;

public class GameToLoad : MonoBehaviour
{
    [SerializeField] private string gameSelected;
    [SerializeField] private TMP_Text progressText;

    private void Awake()
    {
        if (gameSelected == null || gameSelected.Trim() == "")
        {
            Debug.LogError("GameToLoad.cs error: gameSelected is null or empty");
            return;
        }

        if (progressText == null)
        {
            Debug.LogError("GameToLoad.cs error: progressText is null");
            return;
        }

        UpdateProgressDisplay();
    }

    private void UpdateProgressDisplay()
    {
        // Check if the session exists
        if (!SaveManager.SessionExists(gameSelected))
        {
            progressText.text = "Empty";
            Debug.Log($"GameToLoad: Session '{gameSelected}' does not exist - displaying Empty");
            return;
        }

        // Load the session data
        PlayerSesionData sessionData = SaveManager.LoadSession(gameSelected);

        if (sessionData == null)
        {
            progressText.text = "Empty";
            Debug.LogWarning($"GameToLoad: Failed to load session '{gameSelected}' - displaying Empty");
            return;
        }

        // Format the progress text with level, coins, and health
        string levelName = sessionData.level.ToString();
        string formattedLevelName = char.ToUpper(levelName[0]) + levelName.Substring(1).ToLower();

        progressText.text = $"Level: {formattedLevelName} | Coins: {sessionData.coins} | Health: {sessionData.health}";

        Debug.Log($"GameToLoad: Displaying progress for '{gameSelected}' - Level: {formattedLevelName}, Coins: {sessionData.coins}, Health: {sessionData.health}");
    }

    public void LoadGame()
    {
        print("Loading game: " + gameSelected);

        if (string.IsNullOrWhiteSpace(gameSelected))
        {
            Debug.LogError("GameToLoad: Cannot load game - gameSelected is null or empty");
            return;
        }

        // Check if GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameToLoad: GameManager instance not found");
            return;
        }

        // Check if the session exists
        if (!SaveManager.SessionExists(gameSelected))
        {
            Debug.LogWarning($"GameToLoad: Session '{gameSelected}' does not exist. Creating new session.");
            GameManager.Instance.StartNewGameSession(gameSelected);
            return;
        }

        // Load the game through GameManager
        GameManager.Instance.LoadGameSession(gameSelected);
        Debug.Log($"GameToLoad: Requested GameManager to load session '{gameSelected}'");
    }

    public void RefreshDisplay()
    {
        UpdateProgressDisplay();
    }
}
