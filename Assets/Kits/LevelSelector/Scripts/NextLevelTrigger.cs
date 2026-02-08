using System;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Level nextLevel;
    [SerializeField] private bool autoDisableAfterTrigger = true;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasBeenTriggered)
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("NextLevelTrigger: GameManager instance not found");
                return;
            }

            if (!GameManager.Instance.HasActiveSession())
            {
                Debug.LogWarning("NextLevelTrigger: No active session, cannot change level");
                return;
            }

            // Mark as triggered to prevent multiple activations
            if (autoDisableAfterTrigger)
            {
                hasBeenTriggered = true;
            }

            Debug.Log($"NextLevelTrigger: Player entered trigger, transitioning to {nextLevel}");

            // Change to next level - this will:
            // 1. Update the level in session data
            // 2. Reset player position to Vector2.zero (default spawn)
            // 3. Save the session
            // 4. Load the new level scene
            GameManager.Instance.NextLevel(nextLevel);
        }
    }

    public void ResetTrigger()
    {
        hasBeenTriggered = false;
        Debug.Log("NextLevelTrigger: Reset");
    }
}
