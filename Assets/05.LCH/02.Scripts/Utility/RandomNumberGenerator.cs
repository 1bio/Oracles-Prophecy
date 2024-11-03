using System.Collections.Generic;
using UnityEngine;

public static class RandomNumberGenerator
{
    public static int[] GenerateRandomIndex(int maxcount, int n) // �ִ� ����: maxcount, ����: n
    {
        List<int> defaluts = new List<int>();

        for (int i = 0; i < maxcount; i++)
        {
            defaluts.Add(i);
        }

        int[] results = new int[n];

        // ������ �ε��� �̱� (�ߺ� ����)
        for (int i = 0; i < n; i++)
        {
            int randValue = Random.Range(0, defaluts.Count); // ������ �� �� �ϳ� ����
            results[i] = defaluts[randValue]; // ����� �߰�
            defaluts.RemoveAt(randValue); // �ߺ� ������ ���� ���õ� �� ����
        }

        return results;
    }
}

