using System;
using UnityEngine;

public class DropOnHit : BaseCharacter, IVisible2D
{
    //[SerializeField] protected IVisible2D.Side side;
    [SerializeField] IVisible2D.Side[] sidesToAttack = { IVisible2D.Side.PlayerFriends };
    [SerializeField] int priority = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int IVisible2D.GetPriority()
    {
        return priority;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return side;
    }
}
