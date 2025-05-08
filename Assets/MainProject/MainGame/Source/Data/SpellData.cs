using UnityEngine;

namespace MainGame 
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "Data/CombatData/SpellData", order = 0)]
    public class SpellData : ScriptableObject
    {
        [SerializeField] private SpellBase _spellPrefab;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _damageRange;
        [SerializeField] private float _cooldownTime;
        
        public SpellBase SpellPrefab => _spellPrefab;
        public float Speed => _speed;
        public int Damage => _damage;
        public float DamageRange => _damageRange;
        public float CooldownTime => _cooldownTime;
    }
}