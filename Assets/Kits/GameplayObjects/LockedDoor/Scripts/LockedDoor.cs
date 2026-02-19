using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] InventoryItemDefinition requiredKey;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        Inventory inventory = collision.collider.GetComponent<Inventory>();
        if (inventory == null)
            return;

        InventoryItem keyItem = inventory.GetItem(requiredKey.uniqueItemName);

        if (keyItem != null)
        {
            inventory.RemoveItem(keyItem);
            Destroy(gameObject);
        }
    }

}
