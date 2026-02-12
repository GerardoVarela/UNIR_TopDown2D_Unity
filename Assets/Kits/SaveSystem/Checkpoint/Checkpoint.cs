using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private Transform _rangeCheckpoint;
    [SerializeField] private float detectionRadius = 0.35f;
    [SerializeField] private bool autoSaveOnTouch = true;
    [SerializeField] private float saveCooldown = 2f; // Cooldown between saves when autoSave is enabled
    [SerializeField] private string playerTag = "Player";

    private bool hasBeenActivated = false;
    private Transform playerTransform;
    private float lastSaveTime = -999f;

    private void OnDrawGizmos()
    {
        if (_rangeCheckpoint != null)
        {
            Gizmos.color = hasBeenActivated ? Color.green : Color.cyan;
            Gizmos.DrawWireSphere(_rangeCheckpoint.position, detectionRadius);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        // If already activated and autoSave is disabled, don't reactivate
        if (hasBeenActivated && !autoSaveOnTouch)
        {
            return;
        }

        // Check cooldown to avoid saving too frequently
        if (Time.time - lastSaveTime < saveCooldown)
        {
            return;
        }

        // Check if GameManager exists
        if (GameManager.Instance == null)
        {
            // Debug.LogError("Checkpoint: GameManager instance not found - cannot save checkpoint");
            return;
        }

        // Check if there is an active session
        if (!GameManager.Instance.HasActiveSession())
        {
            // Debug.LogWarning("Checkpoint: No active game session - cannot save checkpoint");
            return;
        }

        // Get the respawn point position
        Vector2 respawnPosition = _respawnPoint != null ? _respawnPoint.position : transform.position;

        // Save the checkpoint through GameManager
        GameManager.Instance.SaveCheckpoint(respawnPosition);

        lastSaveTime = Time.time;

        // Mark as activated only on first activation
        if (!hasBeenActivated)
        {
            hasBeenActivated = true;
            // Debug.Log($"Checkpoint: Activated at position {respawnPosition} for session '{GameManager.Instance.CurrentSessionName}'");
            OnCheckpointActivated();
        }
        else
        {
            // Debug.Log($"Checkpoint: Progress saved at position {respawnPosition}");
        }
    }

    protected virtual void OnCheckpointActivated()
    {
        // This can be overridden in derived classes for custom behavior
        // For example: play a sound, show a visual effect, etc.
        // Debug.Log("Checkpoint: OnCheckpointActivated - Add visual/audio feedback here");
    }

    public void ManualActivate()
    {
        if (GameManager.Instance != null && GameManager.Instance.HasActiveSession())
        {
            Vector2 respawnPosition = _respawnPoint != null ? _respawnPoint.position : transform.position;
            GameManager.Instance.SaveCheckpoint(respawnPosition);
            hasBeenActivated = true;
            OnCheckpointActivated();
        }
    }

    public void ResetCheckpoint()
    {
        hasBeenActivated = false;
        lastSaveTime = -999f;
        // Debug.Log("Checkpoint: Reset");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Validate required references
        if (_respawnPoint == null)
        {
            // Debug.LogWarning("Checkpoint: _respawnPoint is not assigned, using checkpoint position as respawn point");
            _respawnPoint = transform;
        }

        if (_rangeCheckpoint == null)
        {
            // Debug.LogWarning("Checkpoint: _rangeCheckpoint is not assigned, using checkpoint position for detection");
            _rangeCheckpoint = transform;
        }

        // Try to find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            // Debug.LogWarning($"Checkpoint: Player with tag '{playerTag}' not found in the scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If player not found yet, try to find it
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
            }
            return;
        }

        // Check if player is within detection range
        if (!hasBeenActivated || autoSaveOnTouch)
        {
            float distanceToPlayer = Vector2.Distance(_rangeCheckpoint.position, playerTransform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                ActivateCheckpoint(playerTransform.gameObject);
            }
        }
    }
}
