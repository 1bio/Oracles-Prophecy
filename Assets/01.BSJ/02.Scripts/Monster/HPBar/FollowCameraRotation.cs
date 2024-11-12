using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FollowCameraRotation : MonoBehaviour
{
    private Monster _monster;
    private MonsterHealthOnMouse _monsterHealthOnMouse;

    private Slider _slider;
    private TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        _monster = GetComponentInParent<Monster>();
        _monsterHealthOnMouse = FindObjectOfType<MonsterHealthOnMouse>();
        _slider = GetComponentInChildren<Slider>();
        _textMeshPro = _slider.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!_monsterHealthOnMouse.OnMouse)
        {
            _slider.gameObject.SetActive(false);
        }
        else if (_monsterHealthOnMouse.Monster == _monster && _monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth >= 0)
        {
            _slider.gameObject.SetActive(true);
        }

        if (_slider.gameObject.activeSelf)
        {
            _slider.maxValue = _monster.CombatController.MonsterCombatAbility.MonsterHealth.MaxHealth;
            _slider.value = _monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth;

            int index = _monster.gameObject.name.IndexOf("_");
            string gameObjectName = _monster.gameObject.name;

            if (index != -1)
            {
                gameObjectName = _monster.gameObject.name.Substring(0, index);
            }
            _textMeshPro.text = $"{gameObjectName}";

            if (_slider.value <= 0)
            {
                _slider.gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(30, 45, 0);
    }
}
