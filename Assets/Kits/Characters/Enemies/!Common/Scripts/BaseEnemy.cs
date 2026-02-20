using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    Sight2D sight;
    private Knockback knockback;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
        knockback = GetComponent<Knockback>();
    }

    protected override void Update()
    {
        base.Update();

        Transform closestTarget = sight.GetClosesTarget();
        if (closestTarget == null)
        {
            RequestStopMoving();

            return;
        }

        Vector2 directionToTarget =
            (closestTarget.position - transform.position).normalized;

        attackDirection = directionToTarget;

        if (!knockback.gettingKnockedBack) Move(directionToTarget);

        float distance =
            Vector2.Distance(transform.position, closestTarget.position);

        if (distance <= attackRange)
        {
            PerformDirectionalAttack();
        }
    }




}
