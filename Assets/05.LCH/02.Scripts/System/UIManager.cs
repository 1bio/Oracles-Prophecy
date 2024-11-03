using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("스킬 선택창 UI 필드")]
    public GameObject selectWindow;
    public GameObject[] selectPosition = new GameObject[2];

    [Header("근거리 스킬 프리팹")]
    public GameObject[] meleeSkillPrefabs;

    [Header("원거리 스킬 프리팹")]
    public GameObject[] rangeSkillPrefabs;

    [Header("스킬 쿨다운")]
    public GameObject[] emptySlot;
    public GameObject[] skillSlot;

    [Header("인벤토리 UI")]
    public GameObject inventoryWindow;

    [Header("플레이어 스탯")]
    public Slider hp_Slider;
    public Slider mana_Slider;
    public Text level;
    public Slider exp_Slider;

    private bool isMelee;

    private string skillName;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        selectWindow.SetActive(true);

        GetRandomSkill();
    }

    private void Update()
    {
        hp_Slider.value = DataManager.instance.playerData.statusData.currentHealth / 100;
        mana_Slider.value = DataManager.instance.playerData.statusData.mana / 100;
        exp_Slider.value = DataManager.instance.playerData.statusData.exp / 100;

        if (Input.GetKeyDown(KeyCode.K))
        {
            SelectWindow(true);

            GetRandomSkill();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryWindow(!inventoryWindow.activeSelf);
        }
    }


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


    #region Main Methods
    public void SetIsMelee(bool isMelee)
    {
        this.isMelee = isMelee;
    }

    // 랜덤 스킬 생성
    public void GetRandomSkill()
    {
        if (isMelee)
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
    }

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

                    case 5: // 회전베기
                        GameObject spinSlashSlot = Instantiate(skillSlot[index], emptySlot[2].transform);
                        spinSlashSlot.transform.SetSiblingIndex(0);
                        break;
                }

                break;
            }
        }
    }
    #endregion
}
