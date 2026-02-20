using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    /*
    int IVisible2D.GetPriority()
    {
        return 0;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return IVisible2D.Side.Neutrals;
    }
    */
    public int GetAmount() { return  amount; } 
    public void NotifyPickedUp()
    {
        Destroy(gameObject);
    }
}
