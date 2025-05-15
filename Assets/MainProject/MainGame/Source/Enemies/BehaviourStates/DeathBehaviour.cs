using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DeathBehaviour : BehaviourStateBase
    {
        private AnimationEventsController _animationEventsController;
        public DeathBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _animationEventsController = enemyController.AnimationEventsController;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enemy dead");

            enemyController.AnimationEventsController.OnDeathFinished += DeathHandler;
        }

        private void DeathHandler()
        {
            Debug.Log("finish death");
            Object.Destroy(enemyController.RootObject);
        }

        public override void Dispose()
        {
        }
    }
}
