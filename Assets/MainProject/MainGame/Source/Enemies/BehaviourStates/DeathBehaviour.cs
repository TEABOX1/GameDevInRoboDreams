using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DeathBehaviour : BehaviourStateBase
    {
        private AnimationEventsController _animationEventsController;
        public DeathBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enemy dead");

            //_animationEventsController.OnDeathFinished += DeathHandler();
        }

        private void DeathHandler()
        {
            Object.Destroy(enemyController.RootObject);
        }

        public override void Dispose()
        {
        }
    }
}
