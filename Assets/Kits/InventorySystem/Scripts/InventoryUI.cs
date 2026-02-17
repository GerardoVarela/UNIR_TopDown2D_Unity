using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference toggleInventory;
    [Header("References")]
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GridLayoutGroup grid;
    

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshUI;
            RefreshUI();
        }


        toggleInventory.action.Enable();
        toggleInventory.action.performed += OnToggleInventory;
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshUI;
        }
        toggleInventory.action.performed -= OnToggleInventory;
        toggleInventory.action.Disable();
    }

    void RefreshUI()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.GetItems())
        {
            GameObject go = Instantiate(inventoryItemPrefab, grid.transform);
            InventoryItemUI ui = go.GetComponent<InventoryItemUI>();
            ui.Init(item, inventory);
        }
    }
}
