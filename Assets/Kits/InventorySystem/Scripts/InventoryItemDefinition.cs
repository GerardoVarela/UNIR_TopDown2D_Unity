using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class InventoryItemDefinition : ItemEffectDefinition
{
    public Sprite image;
    [FormerlySerializedAs("itemName")]
    public string uniqueItemName;
    public string displayName;
    public int numUses = 1;
    public bool usableInUI = true;
    public bool removableInUI = true;
    public GameObject objectToSpawnOnUse;
}
