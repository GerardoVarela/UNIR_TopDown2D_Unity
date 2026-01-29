using System;
using Unity.VisualScripting;
using UnityEngine;

public class Sight2D : MonoBehaviour
{

    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 5f;

    Transform closesPlayer;
    float distanceToClosesPlayer;

    float lastCheckTime = 0.0f;
    Collider2D[] colliders = new Collider2D[0];
    // Update is called once per frame
    void Update()
    {

        if ((Time.time - lastCheckTime) > (1f / checkFrequency))
        {
            lastCheckTime = Time.time;

            colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            closesPlayer = null;
            distanceToClosesPlayer = Mathf.Infinity;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Player"))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (distanceToPlayer < distanceToClosesPlayer)
                    {
                        closesPlayer = colliders[i].transform;
                        distanceToClosesPlayer = distanceToPlayer;
                    }
                }
            }
        }
    }


    public Transform GetClosesTarget()
    {
        return closesPlayer;
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
