using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class InventoryItemDefinition : ScriptableObject
{
    public Sprite image;
    [FormerlySerializedAs("itemName")]
    public string uniqueItemName;
    public float healthRecovery;
    public int bullets;
    public int numUses = 1;
}
