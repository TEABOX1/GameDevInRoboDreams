using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class NecroEnemyController : EnemyController
    {
        [SerializeField] private SpiderSpawnSpell _spiderSpawnSpell;
        [SerializeField] private EnemySpellCaster _enemySpellCaster;

        [Header("AttackMode Settings")]
        [SerializeField] private float _switchToMeleeDistance;
        [SerializeField] private float _switchToRangedDistance;


        public SpiderSpawnSpell SpiderSpawnSpell => _spiderSpawnSpell;
        public EnemySpellCaster EnemySpellCaster => _enemySpellCaster;
        public float ToMeleeDistance => _switchToMeleeDistance;
        public float ToRangedDistance => _switchToRangedDistance;

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
                new NecroAttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));
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

        protected override void HealthDeathHandler()
        {
            base.HealthDeathHandler();
            //ServiceLocator.Instance.GetService<IHealthService>().RemoveCharacter(_health);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _switchToMeleeDistance);

            Color rangedColor = Color.green;
            rangedColor.a = 0.5f;
            Gizmos.color = rangedColor;
            Gizmos.DrawWireSphere(transform.position, _switchToRangedDistance);
        }
    }
}
