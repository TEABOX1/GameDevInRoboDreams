using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class AttackBehaviour : BehaviourStateBase
    {
        public enum AttackState
        {
            Approach = 0,
            Attack =1,

            NullState = 255
        }

        /*private enum AttackMode
        {
            Melee = 0,
            Ranged = 1,

            NullState = 255
        }
        private AttackMode _currentMode;*/

        public event Action<AttackState> OnAttackStateChange;
        //public event Action<AttackMode> OnAttackModeChange;

        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;

        private AttackState _currentState;

        public AttackBehaviour(StateMachine stateMachine, byte stateId, EnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
        }

        public override void Dispose()
        {
        }
    }
}
