using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "EnemyAttackData", menuName = "Data/CombatData/EnemyAttackData", order = 0)]
    public class EnemyAttackData : ScriptableObject
    {
        [SerializeField] private float _interval;
        [SerializeField] private float _distance;
        [SerializeField] private int _damage;

        public float Interval => _interval;
        public float Distance => _distance;
        public int Damage => _damage;
    }
}