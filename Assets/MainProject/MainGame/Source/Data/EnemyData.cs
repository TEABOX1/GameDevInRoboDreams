using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/CombatData/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private Vector2 _idleDuration;
        [SerializeField] private float _maxPatrolStamina;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _chaseSpeed;
        [SerializeField] private float _lookAroundDistance;

        public Vector2 IdleDuration => _idleDuration;
        public float MaxPatrolStamina => _maxPatrolStamina;
        public float PatrolSpeed => _patrolSpeed;
        public float ChaseSpeed => _chaseSpeed;
        public float LookAroundDistance => _lookAroundDistance;
    }
}
