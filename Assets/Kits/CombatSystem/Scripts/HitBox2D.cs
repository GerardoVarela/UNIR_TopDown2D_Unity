using System;
using UnityEngine;

public class HitBox2D : MonoBehaviour
{
    [SerializeField] string affectedTag = "Enemy";
    //Rigidbody2D rb2D;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(affectedTag))
        {
            //MovementController movementController = collision.GetComponent<MovementController>();

            // movementController.NotifyHit(this);
        }
    }


}