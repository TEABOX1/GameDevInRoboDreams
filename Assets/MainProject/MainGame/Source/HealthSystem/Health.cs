using System;
using UnityEngine;

namespace MainGame
{
    public class Health : MonoBehaviour, IHealth
    {
        public event Action OnDeath;
        public event Action<int> OnHealthChanged;
        public event Action<float> OnHealthChanged01;

        [SerializeField] private Collider _characterCollider;
        [SerializeField] private int _maxHealth;

        private int _health;
        private bool _isAlive;

        public int MaxHealthValue => _maxHealth;
        public float HealthValue01 => _maxHealth > 0 ? _health / (float)_maxHealth : 0f;
        public Collider Collider => _characterCollider;

        public int HealthValue
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
                _health = value;
                OnHealthChanged?.Invoke(_health);
                OnHealthChanged01?.Invoke(_health / (float)_maxHealth);
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (_isAlive == value)
                    return;
                _isAlive = value;
                if (!_isAlive)
                    OnDeath?.Invoke();
            }
        }

        protected virtual void Awake()
        {
            SetHealth(MaxHealthValue);
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;
            SetHealth(HealthValue - damage);
        }

        public void Heal(int heal)
        {
            if (!IsAlive) return;
            SetHealth(HealthValue + heal);
        }

        public void SetHealth(int health)
        {
            HealthValue = Mathf.Clamp(health, 0, _maxHealth);
            IsAlive = HealthValue > 0;
        }
    }
}