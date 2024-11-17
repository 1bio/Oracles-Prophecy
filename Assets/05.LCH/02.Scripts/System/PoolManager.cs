using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public GameObject[] prefabs;
    public List<GameObject>[] pools;


    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Awake()
    {
        Init();

        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        } 
    }


    // 투사체 오브젝트 풀링
    public GameObject Get(int index)
    {
        GameObject select = null;

        // 풀 오브젝트 존재 O => 오브젝트 활성화
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;

                select.SetActive(true);

                StartCoroutine(Deactivate(select));

                break;
            }
        }

        // 풀 오브젝트 존재 X => 오브젝트 생성
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);

            StartCoroutine(Deactivate(select));

            pools[index].Add(select);
        }

        return select;
    }


    // 오브젝트 비활성화
    IEnumerator Deactivate(GameObject select) 
    {
        switch (select.tag)
        {
            case "DamageText":
                yield return new WaitForSeconds(0.5f);
                select.SetActive(false);
                break;
        }
    }
}
