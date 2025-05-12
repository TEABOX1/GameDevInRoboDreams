using System;
using System.Collections.Generic;
using GlobalSource;
using MainGame;
using UnityEngine;

namespace MainGame
{
    //TODO: Remove
    public class EnemyTest : MonoBehaviour, IEnemy
    {
        public event Action<IEnemy> OnDied;
        
        [SerializeField] private Health _health;
        [SerializeField] private EnemyTypes _type = EnemyTypes.Spider;
        [SerializeField] private List<Collider> _colliders;
        
        private IHealthService _healthService;
        private EnemyService _enemyService;
        
        public IReadOnlyList<Collider> Colliders => _colliders;
        
        public EnemyTypes EnemyType => _type;
        
        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _healthService.AddCharacter(_health);
            
            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _enemyService.RegisterEnemy(this);

            _health.OnHealthChanged += HealthChangedHandler;
            _health.OnDeath += DeathHandler;
        }

        private void HealthChangedHandler(int health)
        {
            Debug.Log($"{gameObject.name} health changed to {health}");
        }

        private void DeathHandler()
        {
            OnDied?.Invoke(this);
            
            Debug.Log($"{gameObject.name} died");
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            _enemyService.UnregisterEnemy(this);
        }
    }
}