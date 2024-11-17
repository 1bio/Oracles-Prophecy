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
    protected float p_Count = 5;
    protected bool p_HasPotion = false;

    private void Start()
    {
        _countText.text = p_Count.ToString();
    }

    protected void Update()
    {
        CheckPotionCount();
    }

    protected void CheckPotionCount()
    {
        _countText.text = p_Count.ToString();
        p_HasPotion = p_Count > 0;
        _emptyImage.gameObject.SetActive(!p_HasPotion);
    }
}
