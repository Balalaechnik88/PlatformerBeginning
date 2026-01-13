using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;

    public event Action<int, int> HealthChanged;
    public event Action Died;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
        HealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        HealthChanged?.Invoke(CurrentHealth, _maxHealth);

        if (CurrentHealth == 0)
            Died?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, _maxHealth);
        HealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }
}
