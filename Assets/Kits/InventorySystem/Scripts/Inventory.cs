using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] ItemDatabase database;
    [SerializeField] BaseCharacter character;

    private List<InventoryItem> items = new List<InventoryItem>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
         if (character == null)
        {
            character = GetComponent<BaseCharacter>();
        }
    }
    public ItemDatabase GetDatabase()
    {
        return database;
    }

    public void AddItem(InventoryItemDefinition definition)
    {
        InventoryItem newItem =
            new InventoryItem(definition.uniqueItemName, definition.numUses);

        items.Add(newItem);
        OnInventoryChanged?.Invoke();
    }

    public bool Contains(string uniqueName)
    {
        return items.Exists(x => x.uniqueItemName == uniqueName);
    }

    public InventoryItem GetItem(string uniqueName)
    {
        return items.Find(x => x.uniqueItemName == uniqueName);
    }

    public List<InventoryItem> GetItems(string uniqueName)
    {
        return items.FindAll(x => x.uniqueItemName == uniqueName);
    }

    public void UseItem(InventoryItem item)
    {
        if (item == null || !items.Contains(item))
            return;

        ItemEffectDefinition effectDefinition = database.GetDefinition(item.uniqueItemName);
        if (effectDefinition != null)
        {
            character.ApplyItemEffect(effectDefinition);
        }
        item.remainingUses--;

        if (item.remainingUses <= 0)
        {
            RemoveItem(item);
        }
        else
        {
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(InventoryItem item, bool drop = false)
    {
        if (!items.Contains(item))
            return;

        items.Remove(item);

        if (drop)
        {
            GameObject pickablePrefab = database.GetPickable(item.uniqueItemName).gameObject;

            if (pickablePrefab != null)
            {
                Instantiate(
                    pickablePrefab,
                    transform.position,
                    Quaternion.identity
                );
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }
}
