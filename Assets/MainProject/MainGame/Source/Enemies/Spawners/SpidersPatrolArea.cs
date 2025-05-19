using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class SpidersPatrolArea : HordeSpawner
    {
        //public event Action<int> OnEnemyDeath;

        //[SerializeField] private SpidersPool _enemyPool;
        [SerializeField] private SpiderEnemyController _spider;

        private SortedDictionary<float, EnemyController> _spidersCollection = new();

        private int priority;
        private QuestEvents _questEvents;
        private IPlayerService _playerService;

        protected override void Awake()
        {
            enabled = false;
            priority = 0;

            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            // _questEvents.OnStartQuest += (questId) =>
            // {
            //     if (questId == "KillSpidersQuest")
            //     {
            //         QuestStartHandler();
            //     }
            // };
            
            _questEvents.OnQuestStateChange += (Quest quest) =>
            {
                if (quest.QuestInfo.QuestId == "KillSpidersQuest" 
                    && 
                    quest.QuestState == QuestState.InProgress)
                {
                    QuestStartHandler();
                }
            };
        }

        protected override void Update()
        {
            _spidersCollection.Clear();
            for (int i = 0; i < _enemies.Count; i++)
            {
                float distance = (_enemies[i].NavMeshAgent.nextPosition - _playerService.Player.TargetPivot.position).magnitude;
                _spidersCollection.Add(distance, _enemies[i]);
            }

            int priority = 0;

            foreach (KeyValuePair<float, EnemyController> entry in _spidersCollection)
            {
                entry.Value.NavMeshAgent.avoidancePriority = priority;
                entry.Value.NavMeshAgent.speed = 3.5f - priority;
                priority++;
            }
        }

        protected override void SpawnEnemy()
        {
            base.SpawnEnemy();

            var spider = Instantiate(_spider, _hit.position, Quaternion.identity);
            //var spider = _enemyPool.GetEnemy(_hit.position, Quaternion.identity);
            spider.Initialize(this);
            
            spider.NavMeshAgent.avoidancePriority = priority;
            priority++;

            //_healthService.AddCharacter(spider.Health);
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
