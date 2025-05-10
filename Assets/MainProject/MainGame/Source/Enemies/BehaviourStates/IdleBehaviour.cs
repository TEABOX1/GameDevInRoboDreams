using GlobalSource;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class IdleBehaviour : BehaviourStateBase
    {
        private readonly Vector2 _durationBounds;

        private float _time;
        private float _duration;

        public IdleBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _durationBounds = enemyController.Data.IdleDuration;
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            _duration = Random.Range(_durationBounds.x, _durationBounds.y);

            conditions = new List<IStateCondition>
                { new BaseCondition((byte)EnemyBehaviour.Deciding, CompletedCondition) };
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _time += deltaTime;
        }

        public override void Exit()
        {
            base.Exit();
            enemyController.RestorePatrolStamina();
        }

        public override void Dispose()
        {
        }

        private bool CompletedCondition()
        {
            return _time >= _duration;
        }
    }
}
