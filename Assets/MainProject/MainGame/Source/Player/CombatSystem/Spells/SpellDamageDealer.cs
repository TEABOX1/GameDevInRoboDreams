using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class SpellDamageDealer : DamageDealer
    {
        private SpellData _spellData;
        
        protected override void Start()
        {
            base.Start();

            _spellData = ServiceLocator.Instance.GetService<SpellCaster>().SpellData;
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
                Debug.Log(hit.name);
                if(!_healthService.GetHealth(hit, out IHealth health)) continue;
                if (_hasDealtDamage.Contains(health)) continue;
                
                Debug.Log($"Spell dealt damage to {hit.name}");
                
                health.TakeDamage(_spellData.Damage);
                
                _hasDealtDamage.Add(health);
            }
        }
    }
}