using System;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IVisible2D
{
    [SerializeField] float linearSpeed = 1f;

    [SerializeField] int priority = 0;
    [SerializeField] IVisible2D.Side side;

    protected Animator animator;
    Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {

        animator.SetFloat("HorizontalVelocity", lastMoveDirection.x);
        animator.SetFloat("VerticalVelocity", lastMoveDirection.y);
    }

    Vector2 lastMoveDirection;
    protected void Move(Vector2 direction)
    {
        rb2D.position += direction * linearSpeed * Time.deltaTime;
        lastMoveDirection = direction;
    }

    internal void NotifyPunch()
    {
        Destroy(gameObject);
    }

    int IVisible2D.GetPriority()
    {
        return priority;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return side;
    }
}
