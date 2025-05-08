using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class DeathBehaviour : BehaviourStateBase
    {
        public DeathBehaviour(StateMachine stateMachine, byte stateId, IEnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
        }

        public override void Dispose()
        {
        }
    }
}
