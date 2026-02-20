using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] List<GameObject> pickablePrefabs;

    Dictionary<string, GameObject> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, GameObject>();

        foreach (var prefab in pickablePrefabs)
        {
            PickableInventoryItem pickable = prefab.GetComponent<PickableInventoryItem>();

            if (pickable == null)
            {
                Debug.LogError("Prefab sin PickableInventoryItem: " + prefab.name);
                continue;
            }

            var definition = pickable.GetDefinition();

            if (!lookup.ContainsKey(definition.uniqueItemName))
            {
                lookup.Add(definition.uniqueItemName, prefab);
            }
        }
    }

    public InventoryItemDefinition GetDefinition(string uniqueName)
    {
        if (lookup.TryGetValue(uniqueName, out GameObject prefab))
        {
            return prefab
                .GetComponent<PickableInventoryItem>()
                .GetDefinition();
        }

        Debug.LogError("Item not found: " + uniqueName);
        return null;
    }

    public GameObject GetPickable(string uniqueName)
    {
        if (lookup.TryGetValue(uniqueName, out GameObject prefab))
            return prefab;

        Debug.LogError("Item not found: " + uniqueName);
        return null;
    }
}
