using System;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [field: SerializeField] public List<Target> Targets { get; private set; } = new List<Target>();

    [field: SerializeField] public Target CurrentTarget { get; private set; }

    public void Awake()
    {
        Targets.Clear();
    }

    private void Update()
    {
        SelectNearMonster();
    }

    /*// 감지 범위 설정(원거리 및 근거리)
    public void SetRadius(float radius)
    {
        this.Collider.radius = radius; 
    }*/


    #region Collsion Methods
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Target>(out Target target))
            return;

        Targets.Add(target);
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Target>(out Target target))
            return;

        CurrentTarget = null;

        Targets.Remove(target);
    }
    #endregion


    #region Main Methods
    private void SelectNearMonster()
    {
        if (Targets.Count == 0)
            return;

        foreach(Target target in Targets)
        {
            if (target.GetComponent<Health>().isAlive == false)
            {
                Targets.Remove(target);
            }
        }

        CurrentTarget = SetRanking();
    }

    // 거리 비교 순위 정하기
    private Target SetRanking()
    {
        float[] value = new float[Targets.Count]; // 거리 값의 배열
        float[] ranking = new float[Targets.Count]; // 거리 비교한 순위 배열

        int firstIndex = 0; // 순위를 담을 인덱스

        // 비교 값 세팅, 배열 세팅
        for (int i = 0; i < Targets.Count; i++)
        {
            Vector3 offset = Targets[i].transform.position - transform.position;

            float distance = Vector3.SqrMagnitude(offset);

            value[i] = distance;
        }

        // 순위 비교, 배열 세팅
        for (int i = 0; i < Targets.Count; i++)
        {
            ranking[i] = 1f;

            for (int j = 0; j < Targets.Count; j++)
            {
                if (value[i] > value[j])
                {
                    ranking[i]++;
                }
            }
        }

        for (int i = 0; i < ranking.Length; i++) // 인덱스 순서: ranking.Lenght = target.Count
        {
            if (ranking[i] == 1) // 1순위 오브젝트 찾기
            {
                firstIndex = i;
            }
        }

        CurrentTarget = Targets[firstIndex];
        return CurrentTarget;
    }
    #endregion
}
