using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassSelectWindow : MonoBehaviour
{
    [SerializeField] private List<ClassSelectWindowData> elements = new List<ClassSelectWindowData>();
    [SerializeField] private TextMeshProUGUI[] texts;

    public static int classIndex { get; private set; } // 0 = 전사, 1 = 궁수

    // 전사
    public void ArrowLeft()
    {
        // 중복 호출 방지
        if (texts[0].text == "Warrior")
            return;

        // 캐릭터 직업 전환
        elements[0].characterClassUI?.SetActive(true);
        elements[1].characterClassUI?.SetActive(false);

        // 텍스트 전환
        texts[0].text = "Warrior";
        texts[1].text = "Warrior";
        texts[2].text = "전사는 대개 높은 체력과 방어력을 지녀 적의 공격을\r\n" +
                        "흡수하거나 피해를 최소화하면서도 오랜 시간 전투를\r\n" +
                        "지속할 수 있습니다";

        classIndex = 0;
    }

    // 궁수
    public void ArrowRight()
    {
        // 중복 호출 방지
        if (texts[0].text == "Archer")
            return;

        // 캐릭터 직업 전환
        elements[0].characterClassUI?.SetActive(false);
        elements[1].characterClassUI?.SetActive(true);

        // 텍스트 전환
        texts[0].text = "Archer";
        texts[1].text = "Archer";
        texts[2].text = "궁수는 탁월한 기동성을 활용해 적의 공격을 회피하며\r\n " +
                        "전투 상황에서 적절히 거리를 조절해 피해를 줄 수 있어\r\n " +
                        "생존력도 높은 편입니다";

        classIndex = 1;
    }
}

[System.Serializable]
public class ClassSelectWindowData // UI 작업 후 추가 할 예정
{
    [Header("캐릭터 직업 UI")]
    public GameObject characterClassUI = null;
}
