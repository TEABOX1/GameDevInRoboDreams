using UnityEngine;

namespace MainGame 
{
    [CreateAssetMenu(fileName = "EnemySpellData", menuName = "Data/CombatData/EnemySpellData", order = 0)]
    public class EnemySpellData : ScriptableObject
    {
        [SerializeField] private EnemySpellBase _spellPrefab;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _damageRange;
        [SerializeField] private float _cooldownTime;
        
        public EnemySpellBase SpellPrefab => _spellPrefab;
        public float Speed => _speed;
        public int Damage => _damage;
        public float DamageRange => _damageRange;
        public float CooldownTime => _cooldownTime;
    }
}