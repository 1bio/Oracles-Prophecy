using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterLootItemData", menuName = "Data/MonsterLootItemData")]
public class MonsterLootItemData : ScriptableObject
{
    [SerializeField] private GameObject[] _lootItemArray;
    [Range(0, 100)]
    [SerializeField] private float[] _persentArray;

    public Dictionary<GameObject, float> LootItemDropRates { get; private set; }


    public MonsterLootItemData CreateInstance()
    {
        MonsterLootItemData newInstance = Instantiate(this);

        newInstance.LootItemDropRates = new Dictionary<GameObject, float>();
        newInstance.LootItemDropRates.Clear();

        for (int i = 0; i < _lootItemArray.Length; i++)
        {
            if (_persentArray.Length > i)
            {
                newInstance.LootItemDropRates.Add(_lootItemArray[i], _persentArray[i]);
            }
            else
            {
                newInstance.LootItemDropRates.Add(_lootItemArray[i], 0);
            }
        }

        return newInstance;
    }
}
