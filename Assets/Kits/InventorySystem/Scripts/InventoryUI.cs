using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    [SerializeField] PlayerCharacter owner;

    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GridLayoutGroup grid;

    private void Awake()
    {
        if (instance!=null)
        {
            throw new System.Exception("There is more than one inventory UI");
        }
        instance = this;
        if(grid == null)
        {
            grid = GetComponentInChildren<GridLayoutGroup>();
        }
    }
    public void NotifyItemPicked(InventoryItemDefinition itemDefinition)
    {
        GameObject instatiatedPrefab = Instantiate(inventoryItemPrefab, grid.transform);
        InventoryItemUI itemUI = instatiatedPrefab.GetComponent<InventoryItemUI>();
        itemUI?.SetDefinition(itemDefinition);
        
    }

    internal void NotifyInventoryItemUsed(InventoryItemDefinition definition)
    {

    }

    internal bool Contains(InventoryItemDefinition keyDefinition)
    {
        InventoryItemUI[] items = GetComponentsInChildren<InventoryItemUI>();
        return Array.Find(items, x => x.definition.uniqueItemName == keyDefinition.uniqueItemName);
    }

    internal void Consume(InventoryItemDefinition keyDefinition)
    {
        InventoryItemUI[] items = GetComponentsInChildren<InventoryItemUI>();
        InventoryItemUI item = Array.Find(items, x => x.definition.uniqueItemName == keyDefinition.uniqueItemName);
        item.definition.numUses--;
        if(item.definition.numUses <= 0)
        {
            Destroy(item.gameObject);
        }

    }
}
