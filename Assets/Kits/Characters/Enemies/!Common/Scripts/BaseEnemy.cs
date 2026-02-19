using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    Sight2D sight;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();
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
        Move(directionToTarget);

        float distance =
            Vector2.Distance(transform.position, closestTarget.position);

        if (distance <= attackRange)
        {
            PerformDirectionalAttack();
        }
    }




}
