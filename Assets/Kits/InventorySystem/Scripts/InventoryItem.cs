[System.Serializable]
public class InventoryItem
{
    public string uniqueItemName;
    public int remainingUses;

    public InventoryItem(string name, int uses)
    {
        uniqueItemName = name;
        remainingUses = uses;
    }
}

