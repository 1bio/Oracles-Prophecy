using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TripleShotSlot : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;
    [SerializeField] private TextMeshProUGUI coolDownText;

    private void Update()
    {
        SlotCoolDownUpdate("트리플샷");
    }

    public void SlotCoolDownUpdate(string skillName)
    {
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
}
