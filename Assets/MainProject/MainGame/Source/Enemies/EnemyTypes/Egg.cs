using GlobalSource;
using System;
using UnityEngine;

namespace MainGame
{
    public class Egg : MonoBehaviour, IEnemy
    {
        public event Action<IEnemy> OnDeath;

        [SerializeField] private Health _health;
        [SerializeField] private EggController _eggController;

        private IHealthService _healthService;
        private EnemyTypes _type = EnemyTypes.Egg;
        private EnemyService _enemyService;
        private Collider _collider;

        public EnemyTypes EnemyType => _type;
        public Collider Collider => _collider;

        private void Awake()
        {
            _collider = _health.Collider;

            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _healthService.AddCharacter(_health);

            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _enemyService.RegisterEnemy(this);

            _health.OnDeath += DeathHandler;
        }

        private void DeathHandler()
        {
            _enemyService.UnregisterEnemy(this);
            _eggController.Destroy();
            OnDeath?.Invoke(this);
        }
    }
}
