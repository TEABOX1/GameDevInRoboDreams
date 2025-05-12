using GlobalSource;
using UnityEditor;
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
            // Object.Destroy(enemyController.RootObject);
        }

        public override void Dispose()
        {
        }
    }
}
