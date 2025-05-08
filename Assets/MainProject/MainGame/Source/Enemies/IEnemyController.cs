using UnityEngine.AI;
using UnityEngine;

namespace MainGame
{
    public interface IEnemyController
    {
        float PatrolStamina { get; set; }
        NavMeshAgent NavMeshAgent { get; }
        CharacterController CharacterController { get; }
        Transform CharacterTransform { get; }
        IHealth Health { get; }
        IPlayerdar Playerdar { get; }
        INavPointProvider NavPointProvider { get; }

        Vector2 IdleDuration { get; }
        float PatrolSpeed { get; }
        float ChaseSpeed { get; }
        float LookAroundDistance { get; }

        void ComputeBehaviour();
        void RestorePatrolStamina();
    }
}
