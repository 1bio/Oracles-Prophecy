using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectController : MonoBehaviour
{
    [SerializeField] private ItemObject[] _ItemList;
    [Range(0, 100)]
    [SerializeField] private float[] _dropPercent;
    private Dictionary<ItemObject, float> _ItemDropRates = new Dictionary<ItemObject, float>();

    public event Action<string> ItemGetEvent;

    private void OnEnable()
    {
        for (int i = 0; i < _ItemList.Length; i++)
        {
            if (_dropPercent.Length > i)
            {
                _ItemDropRates.Add(_ItemList[i], _dropPercent[i]);
            }
            else
            {
                _ItemDropRates.Add(_ItemList[i], 0);
            }
        }
    }

    public List<ItemObject> GetDropItems()
    {
        List<ItemObject> itemlist = new List<ItemObject>();
        int randPersent;

        if (_ItemDropRates != null)
        {
            foreach (ItemObject item in _ItemDropRates.Keys)
            {
                if (itemlist.Count >= 2) break;

                randPersent = UnityEngine.Random.Range(0, 101);

                if (_ItemDropRates[item] >= randPersent)
                {
                    itemlist.Add(item);
                    ItemGetEvent?.Invoke(item.name);
                }
            }
        }

        return itemlist;
    }
}
