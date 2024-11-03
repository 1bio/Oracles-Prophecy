using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 오브젝트 토글(애니메이션 이벤트)
/// </summary>

public class WeaponToggle : MonoBehaviour
{
    [SerializeField] private GameObject Weapon;
 
    public void EnableWeapon()
    {
        Weapon.SetActive(true);
    }

    public void DisableWeapon()
    {
        Weapon.SetActive(false);
    }
}

