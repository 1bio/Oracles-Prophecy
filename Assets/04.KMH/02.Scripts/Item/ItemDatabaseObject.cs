using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]

public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();


    public void OnAfterDeserialize()
    {
        GetItem.Clear();
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.Id = i;
            GetItem.Add(i, ItemObjects[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        if (GetItem == null || GetItem.Count == 0)
        {
            GetItem = new Dictionary<int, ItemObject>();
        }
    }
}
