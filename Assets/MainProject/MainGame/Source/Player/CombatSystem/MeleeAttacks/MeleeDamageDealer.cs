using UnityEngine;

namespace MainGame
{
    public class MeleeDamageDealer : DamageDealer
    {
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private Transform _weapon;

        [SerializeField] private MeleeAttack _meleeAttack;

        [SerializeField] private Weapon _weaponTrigger;
        
        public WeaponData WeaponData => _weaponData;
        
        private int _damage;
        private float _criticalDamage;
        
        protected override void Start()
        {
            base.Start();
            
            _damage = _weaponData.Damage;
            _criticalDamage = _weaponData.CriticalDamage;

            _weaponTrigger.OnHit += HitHandler;
            
            _weaponTrigger.WeaponCollider.enabled = false;
        }

        // private void Update()
        // {
        //     //TODO: Uncomment after adding animation event
        //     // if (!_canDealDamage) return;
        //     
        //     if (!Physics.Raycast(_weapon.position, _weapon.up,
        //             out var hit, _weaponData.WeaponLenght, _layerMask)) return;
        //     
        //     if (_hasDealtDamage.Contains(hit.collider)) return;
        //
        //     Debug.Log(_meleeAttack.NumberOfAttacks < 3
        //         ?$"{_damage} dealt to {hit.collider.gameObject.name}"
        //         : $"{_criticalDamage} dealt to {hit.collider.gameObject.name}");
        //
        //     _hasDealtDamage.Add(hit.collider);
        // }
        
        //TODO: Add animation events
        public void StartDealDamage()
        {
            _canDealDamage = true;
            _hasDealtDamage.Clear();
            
            //TODO: Uncomment after adding animation event
            // _weaponTrigger.WeaponCollider.enabled = true;
        }
        
        //TODO: Add animation events
        public void StopDealDamage()
        {
            _canDealDamage = false;
            
            //TODO: Uncomment after adding animation event
            // _weaponTrigger.WeaponCollider.enabled = false;
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawLine(_weapon.position, _weapon.position + _weapon.up * _weaponData.WeaponLenght);
        // }

        private void HitHandler(Collider enemyCollider)
        {
            //TODO: Uncomment after adding animation event?
            // if (!_canDealDamage) return;

            if (!_healthService.GetHealth(enemyCollider, out IHealth health)) return;
            if (_hasDealtDamage.Contains(health)) return;

            Debug.Log(_meleeAttack.NumberOfAttacks < 3
                ? $"{_damage} dealt to {enemyCollider.gameObject.name}"
                : $"{_criticalDamage} dealt to {enemyCollider.gameObject.name}");

            if (_meleeAttack.NumberOfAttacks >= 3)
                health.TakeDamage((int)_criticalDamage);
            else
                health.TakeDamage(_damage);
                
            _hasDealtDamage.Add(health);
        }
    }
}