using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthOnMouse : MonoBehaviour
{
    public static Monster Monster { get; set; } = null;
    public bool OnMouse { get; private set; } = false;

    private int _layerMask;

    [SerializeField] private Slider _slider;
    private TextMeshProUGUI _textMeshPro;

    public static float CurrentTime { get; set; } = 0;
    private float _thresholdTime = 0.8f;

    private void Awake()
    {
        _layerMask = (1 << LayerMask.NameToLayer(GameLayers.Monster.ToString()));
        _textMeshPro = _slider.GetComponentInChildren<TextMeshProUGUI>();

        CurrentTime = 0;
    }

    private void Start()
    {
        _slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition), 0.5f, out RaycastHit hit, 100, _layerMask))
        {
            Monster = hit.collider.gameObject.GetComponentInParent<Monster>();

            CurrentTime = 0;
        }
        else
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime > _thresholdTime)
                Monster = null;
        }

        if (Monster != null)
        {
            _slider.gameObject.SetActive(true);

            _slider.maxValue = Monster.CombatController.MonsterCombatAbility.MonsterHealth.MaxHealth;
            _slider.value = Monster.CombatController.MonsterCombatAbility.MonsterHealth.CurrentHealth;

            int index = Monster.gameObject.name.IndexOf("_");
            string gameObjectName = Monster.gameObject.name;

            if (index != -1)
            {
                gameObjectName = Monster.gameObject.name.Substring(0, index);
            }
            _textMeshPro.text = gameObjectName;

            if (_slider.value <= 0)
            {
                _slider.gameObject.SetActive(false);
                Monster = null;
            }
        }
        else
            _slider.gameObject.SetActive(false);
    }
}
