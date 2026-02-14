using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] public InventoryItemDefinition definition;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button[] buttons;
    
    InventoryUI inventoryUI;

    enum ButtonAction
    {
        Discard,
        Use,
        Give,
        Sell,
    }
    private void Awake()
    {
        if (buttons == null || buttons.Length == 0)
        {
            buttons = GetComponentsInChildren<Button>();
        }

        if (image == null)
        {
            image = GetComponentInChildren<Image>();
        }

        inventoryUI = GetComponentInParent<InventoryUI>();

        //definition = Instantiate(definition);
    }
    private void OnEnable()
    {
        buttons[(int)ButtonAction.Discard].onClick.AddListener(OnDiscard);
        buttons[(int)ButtonAction.Use].onClick.AddListener(OnUse);
        buttons[(int)ButtonAction.Give].onClick.AddListener(OnGive);
        buttons[(int)ButtonAction.Sell].onClick.AddListener(OnSell);
    }
    private void OnDisable()
    {
        buttons[(int)ButtonAction.Discard].onClick.RemoveListener(OnDiscard);
        buttons[(int)ButtonAction.Use].onClick.RemoveListener(OnUse);
        buttons[(int)ButtonAction.Give].onClick.RemoveListener(OnGive);
        buttons[(int)ButtonAction.Sell].onClick.RemoveListener(OnSell);
    }

    private void Start()
    {
        Init(definition);
    }

    public void SetDefinition(InventoryItemDefinition newDefinition)
    {
        definition = Instantiate(newDefinition);
        //Init(definition);
    }
    private void Init(InventoryItemDefinition definition)
    {
        image.sprite = definition.image;
        text.text = definition.uniqueItemName;

    }


    void OnDiscard()
    {
        Debug.Log("OnDiscard", gameObject);
    }

    void OnUse()
    {
        Debug.Log("OnUse", gameObject);
        inventoryUI.NotifyInventoryItemUsed(definition);
        definition.numUses--;
        if (definition.numUses <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnGive()
    {
        Debug.Log("OnGive", gameObject);
    }
    void OnSell()
    {
        Debug.Log("OnSell", gameObject);
    }
}
