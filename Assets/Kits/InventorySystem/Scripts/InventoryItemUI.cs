using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject useButtonParent;
    [SerializeField] Button useButton;
    [SerializeField] GameObject dropButtonParent;
    [SerializeField] Button dropButton;

    InventoryItem item;
    Inventory inventory;

    public void Init(InventoryItem newItem, Inventory inv)
    {
        item = newItem;
        inventory = inv;
        InventoryItemDefinition definition = inventory.GetDatabase().GetDefinition(item.uniqueItemName);

        
        if (definition != null)
        {
            image.sprite = definition.image;
            text.text = definition.displayName + " (" + item.remainingUses + ")";
            useButtonParent.SetActive(definition.usableInUI);
            useButton.enabled = definition.usableInUI;
            dropButtonParent.SetActive(definition.removableInUI);
            dropButton.enabled = definition.removableInUI;
        }


        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();

        useButton.onClick.AddListener(OnUseClicked);
        dropButton.onClick.AddListener(OnDiscardClicked);
    }

    void OnUseClicked()
    {
        SoundManager.Instance?.PlayUI(UIClipType.ButtonClick);
        inventory.UseItem(item);
    }

    void OnDiscardClicked()
    {
        SoundManager.Instance?.PlayUI(UIClipType.ButtonClick);
        inventory.RemoveItem(item, true);
    }
}
