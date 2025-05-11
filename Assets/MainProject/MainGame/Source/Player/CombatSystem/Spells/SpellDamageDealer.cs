using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class SpellDamageDealer : DamageDealer
    {
        private SpellData _spellData;
        EnemyService _enemyService;
        
        protected override void Start()
        {
            base.Start();

            _spellData = ServiceLocator.Instance.GetService<SpellCaster>().SpellData;
            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            ServiceLocator.Instance.GetService<SpellInventory>().OnSpellUnlocked += SpellSetHandler;
        }

        private void SpellSetHandler(SpellData spellData)
        {
            _spellData = spellData;
        }
        
        public void DealSpellDamage(Vector3 center)
        {
            _hasDealtDamage.Clear();
            
            // Collider[] hits = new Collider[10];
            // int hitCount = Physics.OverlapSphereNonAlloc(center, _spellData.DamageRange, hits, _layerMask);
            
            Collider[] hits = Physics.OverlapSphere(center, _spellData.DamageRange, _layerMask);
            
            Debug.Log(hits.Length);
            
            for (int i = 0; i < hits.Length ; i++)
            {
                Collider hit = hits[i];
                if(!_healthService.GetHealth(hit, out IHealth health)) continue;
                if (_hasDealtDamage.Contains(health)) continue;
                
                var damage = _spellData.Damage;
                
                // if (_enemyService.TryGetEnemy(hit, out var enemy))
                // {
                //     if (enemy.EnemyType == EnemyTypes.Spider)
                //     {
                //         damage *= 2;
                //     }
                // }

                if (_enemyService.GetEnemyType(hit) == EnemyTypes.Spider)
                {
                    damage *= 2;
                }
                
                health.TakeDamage(damage);
                
                _hasDealtDamage.Add(health);
            }
        }
    }
}