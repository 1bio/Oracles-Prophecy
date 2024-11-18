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


    // ����ü ������Ʈ Ǯ��
    public GameObject Get(int index)
    {
        GameObject select = null;

        // Ǯ ������Ʈ ���� O => ������Ʈ Ȱ��ȭ
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

        // Ǯ ������Ʈ ���� X => ������Ʈ ����
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);

            StartCoroutine(Deactivate(select));

            pools[index].Add(select);
        }

        return select;
    }


    // ������Ʈ ��Ȱ��ȭ
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
