using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    //TODO: Remove
    public class EnemyTest : MonoBehaviour, IEnemy
    {
        public event Action<IEnemy> OnDeath;
        
        [SerializeField] private Health _health;
        [SerializeField] private EnemyTypes _type = EnemyTypes.Spider;
        [SerializeField] private List<Collider> _colliders;
        
        private EnemyService _enemyService;
        private Collider _collider; //щоб прибрати конфлікти

        public IReadOnlyList<Collider> Colliders => _colliders;
        
        public EnemyTypes EnemyType => _type;
        public Collider Collider => _collider; //щоб прибрати конфлікти

        private void Start()
        {
            _collider = _colliders[0]; //щоб прибрати конфлікти

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
            OnDeath?.Invoke(this);
            
            Debug.Log($"{gameObject.name} died");
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            _enemyService.UnregisterEnemy(this);
        }
    }
}