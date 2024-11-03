using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationWeapon : MonoBehaviour
{
    public float moveSpeed = 1f; 

    private float yAngle;

    void Update()
    {
        yAngle += Time.deltaTime;
        transform.rotation = Quaternion.Euler(90f, yAngle * moveSpeed, 0f);
    }
}
