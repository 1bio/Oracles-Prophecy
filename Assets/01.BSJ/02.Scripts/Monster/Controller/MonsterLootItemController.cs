using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLootItemController
{
    private MonsterLootItemData _monsterLootItemData;

    public MonsterLootItemController(MonsterLootItemData lootItemData)
    {
        _monsterLootItemData = lootItemData.CreateInstance();
    }

    private List<GameObject> GetLootItems()
    {
        List<GameObject> lootlist = new List<GameObject>();
        int randPersent;

        if (_monsterLootItemData.LootItemDropRates != null)
        {
            foreach (GameObject item in _monsterLootItemData.LootItemDropRates.Keys)
            {
                randPersent = Random.Range(0, 101);

                if (_monsterLootItemData.LootItemDropRates[item] >= randPersent)
                {
                    lootlist.Add(item);
                }
            }
        }
        
        return lootlist;
    }

    public void DropLootItems(Vector3 monsterPosition)
    {
        List<GameObject> lootItems = GetLootItems();

        if (lootItems == null || lootItems.Count <= 0)
            return;

        foreach (GameObject lootItem in lootItems)
        {
            Vector3 dropPosition = monsterPosition + Vector3.up / 10 + new Vector3(Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1.5f, 1.5f));

            GameObject droppedItem = GameObject.Instantiate(lootItem, dropPosition, Quaternion.identity);

            droppedItem.transform.position = dropPosition;
        }
    }
}
