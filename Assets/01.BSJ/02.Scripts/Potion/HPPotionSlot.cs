using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotionSlot : PotionSlot
{
    [SerializeField] private float _healAmount;

    private new void Update()
    {
        base.Update();

        if (p_HasPotion && Input.GetKeyUp(KeyCode.Q))
        {
            float playerCurrentHealth = DataManager.instance.playerData.statusData.currentHealth;
            float playerMaxHealth = DataManager.instance.playerData.statusData.maxHealth;

            DataManager.instance.playerData.statusData.currentHealth = Mathf.Min(playerMaxHealth, playerCurrentHealth + _healAmount);

            p_Count--;
        }
    }
}
