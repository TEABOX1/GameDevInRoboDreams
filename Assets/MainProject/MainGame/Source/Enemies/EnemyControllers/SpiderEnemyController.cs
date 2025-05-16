using GlobalSource;
using System;

namespace MainGame
{
    public class SpiderEnemyController : EnemyController
    {
        protected override void InitStateMachine()
        {
            _behaviourMachine = new StateMachine();

            _behaviourMachine.AddState((byte)EnemyBehaviour.Deciding,
                new DecisionBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Deciding, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Idle,
                new IdleBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Idle, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Patrol,
                new PatrolBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Patrol, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Search,
                new SearchBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Search, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
                new SpiderAttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));
            _behaviourMachine.AddState((byte)EnemyBehaviour.Death,
                new DeathBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Death, this));
        }

        protected override void InitBehaviourTree()
        {
            BehaviourLeaf idleLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Idle);
            BehaviourLeaf patrolLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Patrol);

            BehaviourBranch patrolBranch = new BehaviourBranch(patrolLeaf, idleLeaf, PatrolStaminaCondition);

            BehaviourLeaf attackLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Attack);
            BehaviourLeaf searchLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Search);

            BehaviourBranch failedSearch = new BehaviourBranch(searchLeaf, idleLeaf, HasTargetCondition);

            BehaviourBranch seesTarget = new BehaviourBranch(attackLeaf, searchLeaf, SeesTargetCondition);

            BehaviourBranch hasTarget = new BehaviourBranch(seesTarget, patrolBranch, HasTargetCondition);

            _behaviourTree = new BehaviourTree(hasTarget);

            ComputeBehaviour();
        }

        /*public void ResetEnemy()
        {
            InitStateMachine();
            _behaviourMachine.OnStateChange += StateChangeHandler;
            InitBehaviourTree();
            _behaviourMachine.ForceState((byte)EnemyBehaviour.Deciding);

            _health.SetHealth(_health.MaxHealthValue);

            _navMeshAgent.enabled = true;
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = false;

            PatrolStamina = _data.MaxPatrolStamina;
        }*/
    }
}
