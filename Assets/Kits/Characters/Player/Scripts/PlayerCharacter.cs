
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference punch;


    protected override void Awake()
    {
        base.Awake();

        // Load player position from saved session if available
        LoadPlayerPosition();
    }

    private void LoadPlayerPosition()
    {
        // Check if GameManager exists and has an active session
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("PlayerCharacter: GameManager not found, using default position");
            return;
        }

        if (!GameManager.Instance.HasActiveSession())
        {
            Debug.Log("PlayerCharacter: No active session, using default position");
            return;
        }

        PlayerSesionData sessionData = GameManager.Instance.CurrentSessionData;

        if (sessionData == null)
        {
            Debug.LogWarning("PlayerCharacter: Session data is null, using default position");
            return;
        }

        // Check if the player position has been set (not default Vector2.zero)
        if (sessionData.playerPosition != Vector2.zero)
        {
            transform.position = sessionData.playerPosition;
            Debug.Log($"PlayerCharacter: Loaded position from save: {sessionData.playerPosition}");
        }
        else
        {
            Debug.Log($"PlayerCharacter: No saved position found, using default spawn position: {transform.position}");
        }
    }

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        punch.action.Enable();
        punch.action.performed += OnPunch;
    }


    protected override void Update()
    {
        base.Update();

        // Leer los inputs
        Move(rawMove);
        if (mustPunch)
        {
            mustPunch = false;
            PerformPunch();
        }

        // Update player position in GameManager periodically
        UpdatePositionInGameManager();
    }
    
    private float lastPositionUpdateTime = 0f;
    private float positionUpdateInterval = 1f; // Update every 1 second

    private void UpdatePositionInGameManager()
    {
        // Only update every positionUpdateInterval seconds
        if (Time.time - lastPositionUpdateTime < positionUpdateInterval)
            return;

        lastPositionUpdateTime = Time.time;

        // Update GameManager with current position if session is active
        if (GameManager.Instance != null && GameManager.Instance.HasActiveSession())
        {
            GameManager.Instance.UpdatePlayerPosition(transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop)
        {
            ApplyItemEffect(drop.dropDefinition);
            drop.NotifyPickedUp();
        }
    }
    private void PerformPunch()
    {
        PerformDirectionalAttack();
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        punch.action.Disable();
        punch.action.performed -= OnPunch;
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();
        if (rawMove.magnitude > 0f)
        {
            attackDirection = rawMove.normalized;
        }
    }

    bool mustPunch;
    private void OnPunch(InputAction.CallbackContext context)
    {
        mustPunch = true;
    }
}
