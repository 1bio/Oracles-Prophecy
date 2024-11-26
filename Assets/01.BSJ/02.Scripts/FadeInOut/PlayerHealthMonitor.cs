using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthMonitor : MonoBehaviour
{
    [SerializeField] private ObjectFadeInOut _objectFadeInOut;
    [SerializeField] private Health _playerHealth;

    private void OnEnable()
    {
        _playerHealth.ImpactEvent += OnImpact;
    }

    private void OnDisable()
    {
        _playerHealth.ImpactEvent -= OnImpact;
    }

    public void OnImpact()
    {
        _objectFadeInOut.StartFadeInOut(0.5f, 0, 0.3f);
    }
}