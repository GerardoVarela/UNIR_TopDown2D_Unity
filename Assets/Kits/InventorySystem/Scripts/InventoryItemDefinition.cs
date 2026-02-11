using UnityEngine;

[CreateAssetMenu()]
public class InventoryItemDefinition : ScriptableObject
{
    public Sprite image;
    public string itemName;
    public float healthRecovery;
    public int bullets;
}
