using GlobalSource;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class AttackBehaviour : BehaviourStateBase
    {
        public enum AttackState
        {
            Approach,
            Attack
        }

        /*private enum AttackMode { Melee, Ranged }
        private AttackMode _currentMode;*/

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
