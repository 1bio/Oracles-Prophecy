using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthOnMouse : MonoBehaviour
{
    public Monster Monster { get; private set; }
    public bool OnMouse { get; private set; } = false;

    private int _layerMask;

    [SerializeField] private Slider _slider;
    private TextMeshProUGUI _textMeshPro;

    private float _currentTime = 0;

    private void Awake()
    {
        _layerMask = (1 << LayerMask.NameToLayer(GameLayers.Monster.ToString()));
        _textMeshPro = _slider.GetComponentInChildren<TextMeshProUGUI>();

        _currentTime = 0;
    }

    private void Start()
    {
        _slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        OnMouse = Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition), 0.5f, out RaycastHit hit, 100, _layerMask);

        if (OnMouse)
        {
            Monster = hit.collider.gameObject.GetComponentInParent<Monster>();

            if (Monster != null)
                _slider.gameObject.SetActive(true);

            _currentTime = 0;
        }
        else
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > 0.5f)
                _slider.gameObject.SetActive(false);
        }

        if (Monster != null && Monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth >= 0)
        {
            _slider.maxValue = Monster.CombatController.MonsterCombatAbility.MonsterHealth.MaxHealth;
            _slider.value = Monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth;

            int index = Monster.gameObject.name.IndexOf("_");
            string gameObjectName = Monster.gameObject.name;

            if (index != -1)
            {
                gameObjectName = Monster.gameObject.name.Substring(0, index);
            }
            _textMeshPro.text = $"{gameObjectName} : {_slider.value}";

            if (_slider.value <= 0)
            {
                _slider.gameObject.SetActive(false);
                Monster = null;
            }
        }
    }
}
