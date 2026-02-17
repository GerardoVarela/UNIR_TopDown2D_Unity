using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] InventoryItemDefinition keyDefinition;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Colisiona");
            Debug.Log(InventoryUI.instance.Contains(keyDefinition));
            if (InventoryUI.instance.Contains(keyDefinition))
            {
                InventoryUI.instance.Consume(keyDefinition);
                Destroy(gameObject);
            }
        }
    }
}
