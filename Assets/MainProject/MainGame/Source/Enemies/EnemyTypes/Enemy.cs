using GlobalSource;
using System;
using UnityEngine;

namespace MainGame
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        public event Action<IEnemy> OnDeath;

        [SerializeField] private EnemyTypes _type;
        [SerializeField] private Health _health;
        [SerializeField] private EnemyController _enemyController;

        private EnemyService _enemyService;
        private Collider _collider;

        public EnemyTypes EnemyType => _type;
        public Collider Collider => _collider;
        public EnemyController EnemyController => _enemyController;

        private void Awake()
        {
            _collider = _health.Collider;

            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _enemyService.RegisterEnemy(this);

            _health.OnDeath += DeathHandler;
        }

        private void DeathHandler()
        {
            OnDeath?.Invoke(this);
        }

        private void OnDestroy()
        {
            _health.OnDeath -= DeathHandler;
            _enemyService.UnregisterEnemy(this);
        }
    }
}
