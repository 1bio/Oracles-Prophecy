using UnityEngine;

[System.Serializable]
public class MonsterHealth
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public MonsterHealth(float currentHealth, float maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
    }

    public float  CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float  MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public void InitializeHealth()
    {
        _currentHealth = _maxHealth;
    }
}
