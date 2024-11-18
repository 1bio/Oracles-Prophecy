using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPotionSlot : PotionSlot
{
    [SerializeField] private float _manaHealAmount;

    private new void Update()
    {
        base.Update();

        if (p_HasPotion && Input.GetKeyUp(KeyCode.E))
        {
            float playerCurrentMana = DataManager.instance.playerData.statusData.currentMana;
            float playerMaxMana = DataManager.instance.playerData.statusData.maxMana;

            DataManager.instance.playerData.statusData.currentHealth = Mathf.Min(playerMaxMana, playerCurrentMana + _manaHealAmount);

            _count--;
        }
    }
}
