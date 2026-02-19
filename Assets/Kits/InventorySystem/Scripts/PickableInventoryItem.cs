using UnityEngine;

public class PickableInventoryItem : MonoBehaviour
{
    [SerializeField] InventoryItemDefinition itemDefinition;
    [SerializeField] float pickupDelay = 0.3f;

    public InventoryItemDefinition GetDefinition()
    {
        return itemDefinition;
    }

    bool canBePicked = false;

    private void Start()
    {
        Invoke(nameof(EnablePickup), pickupDelay);
    }

    void EnablePickup()
    {
        canBePicked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBePicked)
            return;

        if (!other.CompareTag("Player"))
            return;

        Inventory inventory = other.GetComponent<Inventory>();
        inventory?.AddItem(itemDefinition);

        Destroy(gameObject);
    }
}
