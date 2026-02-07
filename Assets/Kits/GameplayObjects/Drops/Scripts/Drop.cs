using System;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] public DropDefinition dropDefinition;

    internal void NotifyPickedUp()
    {
        Destroy(gameObject);
    }
}
