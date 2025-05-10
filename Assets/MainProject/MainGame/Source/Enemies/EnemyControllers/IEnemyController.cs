using UnityEngine.AI;
using UnityEngine;

namespace MainGame
{
    public interface IEnemyController
    {
        float PatrolStamina { get; set; }
        EnemyData Data { get; }
        EnemyAttack EnemyAttack { get; }
        NavMeshAgent NavMeshAgent { get; }
        CharacterController CharacterController { get; }
        Transform CharacterTransform { get; }
        IHealth Health { get; }
        IPlayerRadar PlayerRadar { get; }
        INavPointProvider NavPointProvider { get; }

        void ComputeBehaviour();
        void RestorePatrolStamina();
    }
}
