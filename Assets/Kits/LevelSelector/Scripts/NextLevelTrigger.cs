using System;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Level nextLevel;
    [SerializeField] private bool autoDisableAfterTrigger = true;
    
    // [Header("Optional Baktracking Settings")]
    // [SerializeField] private bool backtrackingLevel = false;
    // [SerializeField] private Transform backtrackingPoint;

    private bool _hasBeenTriggered = false;

    // public void OnDrawGizmos()
    // {
    //     if(!backtrackingPoint) return;

    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(backtrackingPoint.position, 0.1f);
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasBeenTriggered)
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
                _hasBeenTriggered = true;
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
        _hasBeenTriggered = false;
        Debug.Log("NextLevelTrigger: Reset");
    }
}
