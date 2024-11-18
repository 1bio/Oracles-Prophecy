using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashSlashSlot : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;
    [SerializeField] private TextMeshProUGUI coolDownText;

    private void Update()
    {
        SlotCoolDownUpdate("Single Slash");
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
