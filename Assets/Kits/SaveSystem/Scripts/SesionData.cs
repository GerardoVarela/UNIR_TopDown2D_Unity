using UnityEngine;

public class SesionData : MonoBehaviour
{
    private static SesionData _instance;
    public static SesionData Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SesionData");
                _instance = go.AddComponent<SesionData>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Runtime data (not saved to disk)
    [Header("Runtime Session Data")]
    [SerializeField] private float sessionStartTime;
    [SerializeField] private int enemiesDefeated;
    [SerializeField] private int itemsCollected;
    [SerializeField] private bool isPaused;

    // Properties
    public float SessionStartTime => sessionStartTime;
    public int EnemiesDefeated => enemiesDefeated;
    public int ItemsCollected => itemsCollected;
    public bool IsPaused => isPaused;
    public float PlayTime => Time.time - sessionStartTime;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSession();
    }

    private void InitializeSession()
    {
        sessionStartTime = Time.time;
        enemiesDefeated = 0;
        itemsCollected = 0;
        isPaused = false;

        Debug.Log("SesionData: Session initialized");
    }

    public void ResetSession()
    {
        InitializeSession();
        Debug.Log("SesionData: Session reset");
    }

    public void AddEnemyDefeated()
    {
        enemiesDefeated++;
        Debug.Log($"SesionData: Enemy defeated! Total: {enemiesDefeated}");
    }

    public void AddItemCollected(int amount = 1)
    {
        itemsCollected += amount;
        Debug.Log($"SesionData: Items collected! Total: {itemsCollected}");
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        Debug.Log($"SesionData: Game {(paused ? "paused" : "unpaused")}");
    }

    public string GetFormattedPlayTime()
    {
        float playTime = PlayTime;
        int hours = Mathf.FloorToInt(playTime / 3600f);
        int minutes = Mathf.FloorToInt((playTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(playTime % 60f);

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    public string GetSessionSummary()
    {
        return $"Play Time: {GetFormattedPlayTime()}\n" +
               $"Enemies Defeated: {enemiesDefeated}\n" +
               $"Items Collected: {itemsCollected}";
    }
}
