using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DeathBehaviour : BehaviourStateBase
    {
        public DeathBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
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
            //ServiceLocator.Instance.GetService<IHealthService>().RemoveCharacter(enemyController.Health);
            Object.Destroy(enemyController.RootObject);
        }

        public override void Dispose()
        {
        }
    }
}
