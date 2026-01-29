
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference punch;

    [Header("Punch data")]
    [SerializeField] float punchRadius = 0.3f;
    [SerializeField] float punchRange = 1f;

    protected override void Awake()
    {
        base.Awake();
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
        if(mustPunch)
        {
            mustPunch = false;
            PerformPunch();
        }
    }

    Vector2 punchDirection = Vector2.down;
    private void PerformPunch()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, punchRadius, punchDirection * punchRange);

        foreach (RaycastHit2D hit in hits)
        {
            animator.SetTrigger("Attack");
            BaseCharacter otherBaseCharacter = hit.collider.GetComponent<BaseCharacter>();
            if (otherBaseCharacter != this)
            {
                otherBaseCharacter?.NotifyPunch();
            }
        }
    }

    //Dibujar gizmos de golpe
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, punchDirection*punchRange);
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
            punchDirection = rawMove.normalized;
        }
    }

    bool mustPunch;
    private void OnPunch(InputAction.CallbackContext context)
    {
        mustPunch = true;
    }
}
