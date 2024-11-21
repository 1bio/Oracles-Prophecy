using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private Image _emptyImage;
    [SerializeField] protected TextMeshProUGUI _countText;
    [SerializeField] protected float _count = 5;
    protected bool p_HasPotion = false;

    private void Start()
    {
        _countText.text = _count.ToString();
    }

    protected void Update()
    {
        CheckPotionCount();
    }

    protected void CheckPotionCount()
    {
        _countText.text = _count.ToString();
        p_HasPotion = _count > 0;
        _emptyImage.gameObject.SetActive(!p_HasPotion);
    }
}
