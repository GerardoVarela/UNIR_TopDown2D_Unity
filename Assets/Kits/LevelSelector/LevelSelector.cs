using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    MEADOW, // PRADERA
    FOREST, // BOSQUE
    CAVE, // CUEVA
    CASTLE, // CASTILLO
}

public class LevelSelector : MonoBehaviour
{
    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadLevel(Level level)
    {
        // Load the selected level scene
        string levelName = level.ToString();
        string convertedLevelName = char.ToUpper(levelName[0]) + levelName.Substring(1).ToLower();
        SceneManager.LoadScene(convertedLevelName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
