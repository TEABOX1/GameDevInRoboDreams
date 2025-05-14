using GlobalSource;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class SearchBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;

        private readonly float _chaseSpeed;
        private readonly float _lookAroundDistance;

        public SearchBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;

            _chaseSpeed = enemyController.Data.ChaseSpeed;
            _lookAroundDistance = enemyController.Data.LookAroundDistance;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.isStopped = false;
            _agent.ResetPath();
            _agent.stoppingDistance = 0.1f;

            float randomSpeed = Mathf.Max(0.1f, Random.Range(_chaseSpeed - 0.75f, _chaseSpeed + 0.75f)); // додано рандомізвцію швидкості 
            enemyController.NavMeshAgent.speed = randomSpeed;

            enemyController.NavMeshAgent.SetDestination(enemyController.PlayerRadar.LastTargetPosition);
            //enemyController.NavMeshAgent.SetDestination(GetCirclePosition(enemyController.PlayerRadar.LastTargetPosition, 1.5f, enemyController.NavMeshAgent.avoidancePriority, 3));
            
            conditions = new List<IStateCondition>
                { new BaseCondition((byte)EnemyBehaviour.Deciding, ArrivedCondition) };
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;
            _characterController.Move(velocity * (deltaTime * _chaseSpeed) + Physics.gravity);
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

            if (_agent.remainingDistance <= _lookAroundDistance)
            {
                enemyController.PlayerRadar.LookAround();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _agent.isStopped = false;
            _agent.ResetPath();
        }

        public override void Dispose()
        {
        }

        private bool ArrivedCondition()
        {
            return _agent.pathPending || _agent.remainingDistance <= _agent.stoppingDistance;
        }

        //метод для розподілу групи ворогів навколо гравця
        private Vector3 GetCirclePosition(Vector3 center, float radius, int index, int totalEnemies)
        {
            float angle = index * (360f / totalEnemies);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            if (NavMesh.SamplePosition(center + dir * radius, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return center;
        }
    }
}
