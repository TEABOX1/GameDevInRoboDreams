using System;
using UnityEngine;

namespace MainGame
{
    public class SpidersPatrolArea : HordeSpawner
    {
        public event Action<int> OnEnemyDeath;

        [SerializeField] private SpidersPool _enemyPool;

        protected override void Awake()
        {
            enabled = false;
            //підписка на подію старту квеста
            // ... += QuestStartHandler();
        }

        protected override void SpawnEnemy()
        {
            base.SpawnEnemy();

            var spider = _enemyPool.GetEnemy(_hit.position, Quaternion.identity);
            spider.Initialize(this);

            _healthService.AddCharacter(spider.Health);
            spider.Health.OnDeath += () => EnemyDeathHandler(spider);

            _enemies.Add(spider);
        }

        private void EnemyDeathHandler(SpiderEnemyController spider)
        {
            _healthService.RemoveCharacter(spider.Health);
            _enemyPool.ReturnEnemy(spider);
            _enemies.Remove(spider);
            OnEnemyDeath?.Invoke(_enemyPool.ActiveCount);
        }
    }
}
