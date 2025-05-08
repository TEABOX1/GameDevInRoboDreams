using System;
using UnityEngine;

namespace MainGame
{
    public interface IHealth
    {
        event Action OnDeath;
        event Action<int> OnHealthChanged;
        event Action<float> OnHealthChanged01;

        Collider Collider { get; }
        int HealthValue { get; }
        int MaxHealthValue { get; }
        float HealthValue01 { get; }
        bool IsAlive { get; }

        void TakeDamage(int damage);
        void Heal(int heal);
        void SetHealth(int value);
    }
}
