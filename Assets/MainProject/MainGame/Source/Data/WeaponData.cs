using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/CombatData/WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _criticalDamageMultiplier;
        [SerializeField] private float _weaponLength;
        
        public int Damage => _damage;
        public float CriticalDamage => _damage * _criticalDamageMultiplier;
        public float WeaponLenght => _weaponLength;
    }
}