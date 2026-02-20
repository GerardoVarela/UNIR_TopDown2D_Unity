using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Inventory inventory;

    private void OnEnable()
    {
        if(inventory == null)
        {
            inventory = FindAnyObjectByType<Inventory>();
        }
        inventory.OnCoinsChanged += UpdateCoinsUI;
    }

    private void OnDisable()
    {
        inventory.OnCoinsChanged -= UpdateCoinsUI;
    }

    private void Start()
    {
    }

    private void UpdateCoinsUI(int newAmount)
    {
        coinsText.text = newAmount.ToString();
    }
}