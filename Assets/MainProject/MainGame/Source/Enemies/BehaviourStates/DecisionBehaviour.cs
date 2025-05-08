using GlobalSource;

namespace MainGame
{
    public class DecisionBehaviour : BehaviourStateBase
    {
        public DecisionBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemyController.ComputeBehaviour();
        }

        public override void Dispose()
        {
        }
    }
}