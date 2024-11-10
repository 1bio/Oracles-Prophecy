using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("스킬 선택창 UI 필드")]
    public GameObject selectWindow;

    [Header("스킬 쿨다운")]
    public GameObject[] emptySlot;
    public GameObject[] skillSlot;

    [Header("인벤토리 UI")]
    public GameObject inventoryWindow;

    [Header("플레이어 스탯")]
    public Text level;
    public Slider hp_Slider;
    public Slider mana_Slider;
    public Slider exp_Slider;

    private bool isMelee; // 클래스 확인

    private string skillName; // 스킬 이름 확인

    // 스탯 필드
    private float currentHp;
    private float currentMana;
    private float currentExp;

    private float lerpSpeed = 3f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateStatus();

        // 스킬창(캐릭터 스탯창이랑 통합 예정)
        if (Input.GetKeyDown(KeyCode.I))
        {
            SelectWindow(!selectWindow.activeSelf);
        }

        // 인벤토리 선택 창
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryWindow(!inventoryWindow.activeSelf);
        }
    }

    #region Main Methods
    public void SetIsMelee(bool isMelee)
    {
        this.isMelee = isMelee;
    }

    // 스탯 업데이트
    private void UpdateStatus()
    {
        level.text = DataManager.instance.playerData.statusData.level.ToString();

        this.currentHp = DataManager.instance.playerData.statusData.currentHealth / 100;
        this.currentMana = DataManager.instance.playerData.statusData.currentMana / 100;
        this.currentExp = DataManager.instance.playerData.statusData.exp / 100;

        hp_Slider.value = Mathf.Lerp(hp_Slider.value, this.currentHp, lerpSpeed * Time.deltaTime);
        mana_Slider.value = Mathf.Lerp(mana_Slider.value, this.currentMana, lerpSpeed * Time.deltaTime);
        exp_Slider.value = Mathf.Lerp(exp_Slider.value, this.currentExp, lerpSpeed * Time.deltaTime);
    }

    /*// 랜덤 스킬 생성
    public void GetRandomSkill()
    {
        if (!isMelee)
        {
            int[] randomValues = RandomNumberGenerator.GenerateRandomIndex(meleeSkillPrefabs.Length, selectPosition.Length);

            for (int i = 0; i < randomValues.Length; i++)
            {
                int randomValue = randomValues[i];

                Instantiate(meleeSkillPrefabs[randomValue], selectPosition[i].transform);
            }
        }
        else
        {
            int[] randomValues = RandomNumberGenerator.GenerateRandomIndex(rangeSkillPrefabs.Length, selectPosition.Length);

            for (int i = 0; i < randomValues.Length; i++)
            {
                int randomValue = randomValues[i];

                Instantiate(rangeSkillPrefabs[randomValue], selectPosition[i].transform);
            }
        }
    }*/

    public void AddSkillSlot(int index)
    {
        for (int i = 0; i < emptySlot.Length; i++)
        {
            if (emptySlot[i].GetComponentInChildren<TextMeshProUGUI>() == null)
            {
                switch (index)
                {
                    case 0: // 정조준
                        GameObject aimingSlot = Instantiate(skillSlot[index], emptySlot[0].transform);
                        aimingSlot.transform.SetSiblingIndex(0);
                        break;

                    case 1: // 트리플샷
                        GameObject tripleShotSlot = Instantiate(skillSlot[index], emptySlot[1].transform);
                        tripleShotSlot.transform.SetSiblingIndex(0);
                        break;

                    case 2: // 화살비
                        GameObject skyFallShotSlot = Instantiate(skillSlot[index], emptySlot[2].transform);
                        skyFallShotSlot.transform.SetSiblingIndex(0);
                        break;

                    case 3: // 도약베기
                        GameObject dashSlashSlot = Instantiate(skillSlot[index], emptySlot[0].transform);
                        dashSlashSlot.transform.SetSiblingIndex(0);
                        break;

                    case 4: // 화염칼
                        GameObject fireBladeSlot = Instantiate(skillSlot[index], emptySlot[1].transform);
                        fireBladeSlot.transform.SetSiblingIndex(0);
                        break;

                    case 5: // 빙결
                        GameObject spinSlashSlot = Instantiate(skillSlot[index], emptySlot[2].transform);
                        spinSlashSlot.transform.SetSiblingIndex(0);
                        break;
                }

                break;
            }
        }
    }
    #endregion


    #region Toggle Window
    // 스킬 선택 UI 토글
    public void SelectWindow(bool openWindow)
    {
        selectWindow.SetActive(openWindow);
    }

    // 인벤토리 UI 토글
    public void InventoryWindow(bool openWindow)
    {
        inventoryWindow.SetActive(openWindow);
    }
    #endregion
}
