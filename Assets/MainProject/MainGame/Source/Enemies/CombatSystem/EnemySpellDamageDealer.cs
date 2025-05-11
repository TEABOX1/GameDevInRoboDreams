using GlobalSource;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EnemySpellDamageDealer : MonoBehaviour
    {
        [SerializeField] private EnemySpellCaster _enemySpellCaster;
        [SerializeField] protected LayerMask _layerMask;

        private EnemySpellData _spellData;
        private IHealthService _healthService;

        private void Start()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _spellData = _enemySpellCaster.EnemySpellData;
        }

        public void DealSpellDamage(Vector3 center)
        {
            Collider[] colliders = Physics.OverlapSphere(center, _spellData.DamageRange, _layerMask);
            
            HashSet<Health> damagedHealths = new HashSet<Health>();

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if (_healthService.GetHealth(collider, out Health health))
                {
                    if (damagedHealths.Contains(health)) continue;
                    damagedHealths.Add(health);
                    health.TakeDamage(_spellData.Damage);
                }
            }
        }
    }
}
