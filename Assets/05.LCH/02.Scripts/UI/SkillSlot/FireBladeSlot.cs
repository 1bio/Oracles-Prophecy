﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireBladeSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image coolDownImage;
    [SerializeField] private TextMeshProUGUI coolDownText;

    private void Update()
    {
        SlotCoolDownUpdate("Fire Blade");
    }

    public void SlotCoolDownUpdate(string skillName)
    {
        UseSkillDuration(skillName);

        float skillCoolDown = SkillManager.instance.ReturnCoolDown(skillName);
        float remainCoolDown = SkillManager.instance.GetRemainingCooldown(skillName);

        if (remainCoolDown <= 0)
        {
            coolDownImage.fillAmount = 0;
            coolDownText.text = " ";
        }
        else
        {
            coolDownImage.fillAmount = skillCoolDown / remainCoolDown;
            coolDownText.text = $"{(int)remainCoolDown}";
        }
    }

    public void UseSkillDuration(string skillName)
    {
        if (SkillManager.instance.IsPassiveActive(skillName))
        {
            Color color = icon.color;
            color.a = 0.45f; // 대략 alpha 값 100 정도
            icon.color = color;
        }
        else
        {
            Color color = icon.color;
            color.a = 0.78f; // 대략 alpha 값 200 정도
            icon.color = color;
        }
    }
}
