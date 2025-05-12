using GlobalSource;
using System;
using UnityEngine;

namespace MainGame
{
    public class EnemyAttackDamageDealer : MonoBehaviour
    {
        //public event Action OnHit;

        [SerializeField] private EnemyAttack _enemyAttack;

        private IHealthService _healthService;

        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _enemyAttack.OnHit += HitHandler;
        }

        private void HitHandler(Collider collider)
        {
            if (_healthService.GetHealth(collider, out IHealth health))
                health.TakeDamage(_enemyAttack.AttackData.Damage);
            //OnHit?.Invoke();
        }
    }
}
