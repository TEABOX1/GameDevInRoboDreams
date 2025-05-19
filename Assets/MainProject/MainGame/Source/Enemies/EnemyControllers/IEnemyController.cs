using UnityEngine.AI;
using UnityEngine;

namespace MainGame
{
    public interface IEnemyController
    {
        enum AttackState
        {
            Approach = 0,
            Attack = 1,

            NullState = 255
        }

        enum AttackMode
        {
            Melee = 0,
            Ranged = 1,

            NullState = 255
        }

        float PatrolStamina { get; set; }
        EnemyData Data { get; }
        EnemyAttack EnemyAttack { get; }
        NavMeshAgent NavMeshAgent { get; }
        CharacterController CharacterController { get; }
        Transform CharacterTransform { get; }
        GameObject RootObject { get; }
        IHealth Health { get; }
        IPlayerRadar PlayerRadar { get; }
        INavPointProvider NavPointProvider { get; }
        AnimationEventsController AnimationEventsController { get; }

        void ComputeBehaviour();
        void RestorePatrolStamina();
        void InvokeAttackState(AttackState state);
        public void InvokeAttackMode(AttackMode mode);


        //void Initialize(INavPointProvider navPointProvider);
        //void ResetEnemy();
    }
}
