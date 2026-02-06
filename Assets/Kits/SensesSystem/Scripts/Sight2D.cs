using System;
using Unity.VisualScripting;
using UnityEngine;

public class Sight2D : MonoBehaviour
{

    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 5f;
    [Space]
    [SerializeField] IVisible2D.Side[] perceivedSides;

    Transform closestTarget;
    float distanceToClosestTarget;
    int priorityOfClosestTarget;

    float lastCheckTime = 0.0f;
    Collider2D[] colliders = new Collider2D[0];
    // Update is called once per frame
    void Update()
    {

        if ((Time.time - lastCheckTime) > (1f / checkFrequency))
        {
            lastCheckTime = Time.time;

            colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            closestTarget = null;
            distanceToClosestTarget = Mathf.Infinity;
            priorityOfClosestTarget = -1;

            for (int i = 0; i < colliders.Length; i++)
            {
                IVisible2D visible = colliders[i].GetComponent<IVisible2D>();
                if (visible != null && CanSee(visible) && colliders[i].gameObject!=this)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (
                        (visible.GetPriority() > priorityOfClosestTarget)||
                        ((visible.GetPriority() == priorityOfClosestTarget) && (distanceToPlayer < distanceToClosestTarget))
                       )
                    {
                        closestTarget = colliders[i].transform;
                        distanceToClosestTarget = distanceToPlayer;
                        priorityOfClosestTarget = visible.GetPriority();
                    }
                }
            }
        }
    }

    bool CanSee(IVisible2D visible)
    {
        bool canSee = false;

        for (int i = 0; !canSee && (i < perceivedSides.Length); i++)
            { canSee = visible.GetSide() == perceivedSides[i]; }
        return canSee;
    }

    public Transform GetClosesTarget()
    {
        return closestTarget;
    }

    public bool IsPlayerInSight()
    {
        bool isPlayerInSight = false;

        for (int i = 0; !isPlayerInSight && (i < colliders.Length); i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                isPlayerInSight = true;
            }
        }
        return isPlayerInSight;
    }
}
