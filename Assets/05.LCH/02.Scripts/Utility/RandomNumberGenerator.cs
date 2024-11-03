using System.Collections.Generic;
using UnityEngine;

public static class RandomNumberGenerator
{
    public static int[] GenerateRandomIndex(int maxcount, int n) // 최대 개수: maxcount, 수량: n
    {
        List<int> defaluts = new List<int>();

        for (int i = 0; i < maxcount; i++)
        {
            defaluts.Add(i);
        }

        int[] results = new int[n];

        // 랜덤한 인덱스 뽑기 (중복 방지)
        for (int i = 0; i < n; i++)
        {
            int randValue = Random.Range(0, defaluts.Count); // 가능한 값 중 하나 선택
            results[i] = defaluts[randValue]; // 결과에 추가
            defaluts.RemoveAt(randValue); // 중복 방지를 위해 선택된 값 제거
        }

        return results;
    }
}

