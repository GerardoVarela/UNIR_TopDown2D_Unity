using System;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IVisible2D
{
    [SerializeField] float linearSpeed = 1f;

    [SerializeField] int priority = 0;
    [SerializeField] protected IVisible2D.Side side;
    [SerializeField] protected Life life;

    [Header("Directional Attack")]
    [SerializeField] IVisible2D.Side[] sidesToAttack = { IVisible2D.Side.PlayerFriends };
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackRadius = 0.3f;
    [SerializeField] protected float attackDamage = 0.2f;
    [SerializeField] protected float attackCooldown = 1f;
    [Header("Sound")]
    [SerializeField] protected SFXType directionalAttackSoundType = SFXType.Undefined;
    [Header("Idle Delay")]
    [SerializeField] float stopMovingDelay = 0.25f;

    float stopMoveTimer = 0f;
    bool isTryingToStop = false;

    protected Vector2 attackDirection = Vector2.down;

    float lastAttackTime;

    protected Animator animator;
    Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (life == null && !transform.CompareTag("NPC")) { life = GetComponent<Life>(); }
    }

    protected virtual void Update()
    {

        animator.SetFloat("HorizontalVelocity", lastMoveDirection.x);
        animator.SetFloat("VerticalVelocity", lastMoveDirection.y);

        if (isTryingToStop)
        {
            stopMoveTimer += Time.deltaTime;

            if (stopMoveTimer >= stopMovingDelay)
            {
                ForceStopMoving();
            }
        }

    }

    Vector2 lastMoveDirection;
    protected void Move(Vector2 direction)
    {
        rb2D.position += direction * linearSpeed * Time.deltaTime;
        lastMoveDirection = direction;

        isTryingToStop = false; 
    }


    public virtual void NotifyPunch(float damage)
    {
        life?.OnHitReceived(damage);
    }
    protected void PerformDirectionalAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            attackRadius,
            attackDirection.normalized,
            attackRange
        );

        animator?.SetTrigger("Attack");
        SoundManager.Instance?.PlaySFX(directionalAttackSoundType);

        foreach (RaycastHit2D hit in hits)
        {
            BaseCharacter other = hit.collider.GetComponent<BaseCharacter>();

            if (other != null && other != this)
            {
                IVisible2D visible = other as IVisible2D;

                if (visible != null)
                {
                    IVisible2D.Side otherSide = visible.GetSide();

                    bool canBeAttacked = false;

                    foreach (var side in sidesToAttack)
                    {
                        if (side == otherSide)
                        {
                            canBeAttacked = true;
                            break;
                        }
                    }

                    if (canBeAttacked)
                    {
                        other.NotifyPunch(attackDamage);
                    }
                }

            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(attackDirection.normalized * attackRange);

        Gizmos.DrawWireSphere(start, attackRadius);
        Gizmos.DrawWireSphere(end, attackRadius);

        Vector3 rightOffset =
            Vector3.Cross(attackDirection.normalized, Vector3.forward) * attackRadius;

        Gizmos.DrawLine(start + rightOffset, end + rightOffset);
        Gizmos.DrawLine(start - rightOffset, end - rightOffset);
    }
    public virtual void ApplyItemEffect(ItemEffectDefinition itemEffectDefinition)
    {
        if (itemEffectDefinition)
        {
            life.RecoverHealth(itemEffectDefinition.healthRecovery);
        }
    }

    protected void RequestStopMoving()
    {
        if (!isTryingToStop)
        {
            isTryingToStop = true;
            stopMoveTimer = 0f;
        }
    }
    void ForceStopMoving()
    {
        lastMoveDirection = Vector2.zero;
        isTryingToStop = false;
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
