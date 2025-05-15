using GlobalSource;
using System;
using UnityEngine;

namespace MainGame
{
    public class SpidersPatrolArea : HordeSpawner
    {
        //public event Action<int> OnEnemyDeath;

        //[SerializeField] private SpidersPool _enemyPool;
        [SerializeField] private SpiderEnemyController _spider;

        private int priority;
        private QuestEvents _questEvents;

        protected override void Awake()
        {
            enabled = false;
            priority = 0;

            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _questEvents.OnStartQuest += (questId) =>
            {
                if (questId == "KillSpidersQuest")
                {
                    QuestStartHandler();
                }
            };
        }

        protected override void SpawnEnemy()
        {
            base.SpawnEnemy();

            var spider = Instantiate(_spider, _hit.position, Quaternion.identity);
            //var spider = _enemyPool.GetEnemy(_hit.position, Quaternion.identity);
            spider.Initialize(this);
            
            spider.NavMeshAgent.avoidancePriority = priority;
            priority++;

            _healthService.AddCharacter(spider.Health);
            spider.Health.OnDeath += () => EnemyDeathHandler(spider);

            _enemies.Add(spider);
        }

        private void EnemyDeathHandler(SpiderEnemyController spider)
        {
            _enemies.Remove(spider);
            //_enemyPool.ReturnEnemy(spider);
            //OnEnemyDeath?.Invoke(EnemyCount);
        }
    }
}
