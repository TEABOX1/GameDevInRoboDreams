using GlobalSource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MainGame.EnemyController;

namespace MainGame
{
    public class PatrolBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;
        private readonly INavPointProvider _navPointProvider;

        private readonly float _patrolSpeed;

        public PatrolBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
            _navPointProvider = enemyController.NavPointProvider;

            _patrolSpeed = enemyController.PatrolSpeed;
        }

        public override void Enter()
        {
            base.Enter();

            enemyController.NavMeshAgent.speed = _patrolSpeed;
            enemyController.NavMeshAgent.SetDestination(_navPointProvider.GetPoint());

            conditions = new List<IStateCondition>
                { new BaseCondition((byte)EnemyBehaviour.Deciding, ArrivedCondition) };
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;
            _characterController.Move(velocity * (deltaTime * _patrolSpeed) + Physics.gravity);
            Vector3 newPosition = _characterTransform.position;
            Vector3 direction = newPosition - position;
            float distance = (newPosition - position).magnitude;
            _agent.nextPosition = newPosition;
            enemyController.PatrolStamina -= distance;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            direction.Normalize();
            if (!Mathf.Approximately(direction.magnitude, 0f))
            {
                _characterTransform.rotation = Quaternion.LookRotation(direction);
            }
        }

        public override void Dispose()
        {
        }

        private bool ArrivedCondition()
        {
            return _agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance;
        }
    }
}