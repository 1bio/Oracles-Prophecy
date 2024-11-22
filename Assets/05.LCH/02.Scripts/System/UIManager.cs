using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("스킬 트리")]
    public GameObject[] selectWindow; // [0]: 전사, [1]: 궁수
    public TextMeshProUGUI[] skillPoint;

    [Header("인벤토리 UI")]
    public GameObject inventoryWindow;
    public Text[] inventoryStatus;

    // 하단 인터페이스
    [Header("플레이어 스탯")]
    public Text level;
    public Slider hp_Slider;
    public Slider mana_Slider;
    public Slider exp_Slider;

    [Header("스킬 슬롯")]
    public GameObject[] emptySlot;
    public GameObject[] skillSlot;

    [Header("현재 장소 텍스트")]
    public TextMeshProUGUI zoneName;

    [Header("게임 오버")]
    public CanvasGroup gameoverCG;
    public GameObject ingameUI;

    private bool isMelee; // 클래스 확인
    private string skillName; // 스킬 이름 확인

    // 스탯 필드
    private float currentHp;
    private float currentMana;
    private float currentExp;

    private float lerpSpeed = 3f;

    [SerializeField] private float fadeSpeed;

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
        // 임시
        if (Input.GetKeyDown(KeyCode.C))
        {
            DataManager.instance.playerData.statusData.currentHealth += 50;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            DataManager.instance.LevelUp(50, 50);
        }

        UpdateStatus(); // 하단 스탯
        UpdateInventoryStatus(); // 인벤토리 스탯

        // 인벤토리
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryWindow(!inventoryWindow.activeSelf);
        }

        // 스킬 트리
        switch (isMelee)
        {
            case true: // 전사 스킬 트리
                skillPoint[0].text = $"Unused Skill Points {DataManager.instance.playerData.statusData.skillPoint} point";
                if (Input.GetKeyDown(KeyCode.K)) { SKillWindow(!selectWindow[0].activeSelf); }
                break;

            case false: // 궁수 스킬 트리
                skillPoint[1].text = $"Unused Skill Points {DataManager.instance.playerData.statusData.skillPoint} point";
                if (Input.GetKeyDown(KeyCode.K)) { SKillWindow(!selectWindow[1].activeSelf); }
                break;
        }
    }

    #region Main Methods
    public void SetIsMelee(bool isMelee)
    {
        this.isMelee = isMelee;
    }

    // 하단 UI 업데이트
    public void UpdateStatus()
    {
        level.text = DataManager.instance.playerData.statusData.level.ToString();

        this.currentHp = DataManager.instance.playerData.statusData.currentHealth / DataManager.instance.playerData.statusData.maxHealth;
        this.currentMana = DataManager.instance.playerData.statusData.currentMana / DataManager.instance.playerData.statusData.maxHealth;
        this.currentExp = DataManager.instance.playerData.statusData.exp / 100;

        hp_Slider.value = Mathf.Lerp(hp_Slider.value, this.currentHp, lerpSpeed * Time.deltaTime);
        mana_Slider.value = Mathf.Lerp(mana_Slider.value, this.currentMana, lerpSpeed * Time.deltaTime);
        exp_Slider.value = Mathf.Lerp(exp_Slider.value, this.currentExp, lerpSpeed * Time.deltaTime);
    }

    // 인벤토리 UI 업데이트
    public void UpdateInventoryStatus()
    {
        inventoryStatus[0].text = DataManager.instance.playerData.statusData.level.ToString();
        inventoryStatus[1].text = $"{DataManager.instance.playerData.statusData.minDamage} - {DataManager.instance.playerData.statusData.maxDamage}";
        inventoryStatus[2].text = Mathf.Floor(100f / DataManager.instance.playerData.statusData.defense).ToString() + "%";
        inventoryStatus[3].text = DataManager.instance.playerData.statusData.currentHealth.ToString();
        inventoryStatus[4].text = DataManager.instance.playerData.statusData.currentMana.ToString();
        inventoryStatus[5].text = Mathf.Floor(100f / DataManager.instance.playerData.statusData.moveSpeed).ToString() + "%";
    }

    // 스킬 슬롯 생성
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

    // 현재 위치 디스플레이 텍스트
    public IEnumerator DisplayZoneName()
    {
        Color color = zoneName.color;
        color.a = 0f;
        zoneName.color = color;

        // 텍스트를 씬의 이름으로 설정
        //zoneName.text = SceneManager.GetActiveScene().name; 

        // 알파 값을 1까지 올리기
        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 0.5f;
            zoneName.color = color;
            yield return null;
        }

        color.a = 1f;
        zoneName.color = color;

        // 알파값을 0까지 내리기
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * 0.5f;
            zoneName.color = color;
            yield return null;
        }

        color.a = 0f;
        zoneName.color = color;
    }

    // 게임 오버
    public void GameoverUI()
    {
        gameoverCG.interactable = true;
        
        StartCoroutine(Fade(true));
    }

    // 캔버스 그룹 페이드
    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        ingameUI.SetActive(false);

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * fadeSpeed;
            gameoverCG.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }

    }

    // UI가 켜져 있으면 공격 입력 막기
    public bool IsActiveUI()
    {
        if (inventoryWindow.activeSelf || selectWindow[0].activeSelf || selectWindow[1].activeSelf)
        {
            return false;
        }
        else
        {
            return true;
        }
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

    #endregion


    #region Button Event
    // 게임 오버 후 마을로 돌아가는 이벤트
    public void ReturnToVillage()
    {
        gameoverCG.interactable = false;

        StartCoroutine(Fade(false));
        SceneController.instance.LoadScene("LCH");
    }
    #endregion


    #region Toggle Window
    // 스킬 선택 UI 토글
    public void SKillWindow(bool openWindow)
    {
        switch (isMelee)
        {
            case true: 
                selectWindow[0].SetActive(openWindow);
                break;
            case false: 
                selectWindow[1].SetActive(openWindow);
                break;
        }
    }

    // 인벤토리 UI 토글
    public void InventoryWindow(bool openWindow)
    {
        inventoryWindow.SetActive(openWindow);
    }

    public void BottomIconSkill()
    {
        switch (isMelee)
        {
            case true:
                selectWindow[0].SetActive(true);
                break;
            case false:
                selectWindow[1].SetActive(true);
                break;
        }
    }

    public void BottomIconInventory()
    {
        inventoryWindow.SetActive(true);
    }
    #endregion
}
