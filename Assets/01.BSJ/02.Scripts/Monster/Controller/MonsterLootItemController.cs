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
        float randPersent;

        if (_monsterLootItemData.LootItemDropRates != null)
        {
            foreach (GameObject item in _monsterLootItemData.LootItemDropRates.Keys)
            {
                randPersent = Random.Range(0, 100);

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
            Vector3 dropPosition = monsterPosition + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

            GameObject droppedItem = GameObject.Instantiate(lootItem, dropPosition, Quaternion.identity);

            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * 5f, ForceMode.Impulse);
            }
        }
    }
}
