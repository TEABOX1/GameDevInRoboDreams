using GlobalSource;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MainGame
{
    // Запасний скрипт на випадок якщо бос і павуки будуть в одній локації
    public class Spawner : MonoBehaviour, INavPointProvider
    {
        public event Action<int> OnEnemyDeath;
        public event Action OnQuestCompleted;
        public event Action OnBossDeath;

        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected float _spawnRadius;
        [SerializeField] protected Vector3 _offset;

        [SerializeField] protected int _spawnCount = 3;

        [SerializeField] private NecroEnemyController _boss;
        [SerializeField] private SpidersPool _enemyPool;

        protected Vector3 _point;
        protected NavMeshHit _hit;

        protected IHealthService _healthService;

        private void Awake()
        {
            enabled = false;
            //підписка на подію старту квеста на вбивство павуків
            // ... += QuestStartHandler();

        }

        private void OnEnable()
        {
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
            SpawnSpiders(_spawnCount);
        }

        private void Update()
        {
            if (_enemyPool.ActiveCount == 0)
            {
                OnQuestCompleted?.Invoke();
                SpawnBoss();
            }
        }

        private void SpawnSpiders(int count)
        {
            for (int i = 0; i < count; ++i)
                SpawnSpider();
        }

        private void SpawnSpider()
        {
            GetPointInternal();

            int depth = 0;
            while (!_hit.hit)
            {
                GetPointInternal();
                depth++;
                if (depth > 100000)
                {
                    Debug.LogError("Point sampling reached 100000 iterations, aborting");
                    return;
                }
            }

            var enemy = _enemyPool.GetEnemy(_hit.position, Quaternion.identity);
            enemy.Initialize(this);

            _healthService.AddCharacter(enemy.Health);
            enemy.Health.OnDeath += () => EnemyDeathHandler(enemy);
        }

        private void SpawnBoss()
        {
            Vector3 spawnPoint = GetExactPoint(_spawnPoint.position);
            NecroEnemyController boss = Instantiate(_boss, spawnPoint, _spawnPoint.rotation);
            boss.Initialize(this);

            _healthService.AddCharacter(boss.Health);
            boss.Health.OnDeath += () => BossDeathHandler(); //можливо перенести цю подію в EnemyController
        }

        public Vector3 GetPoint()
        {
            GetPointInternal();
            return _hit.position;
        }

        private void GetPointInternal()
        {
            Vector3 center = transform.position + _offset;
            Vector2 randomInCircle = Random.insideUnitCircle * _spawnRadius;
            _point.x = randomInCircle.x + center.x;
            _point.y = center.y;
            _point.z = randomInCircle.y + center.z;
            NavMesh.SamplePosition(_point, out _hit, 1.0f, NavMesh.AllAreas);
        }

        public Vector3 GetExactPoint(Vector3 targetPosition)
        {
            NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas);
            return hit.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position + _offset;

            Gizmos.DrawWireSphere(center, _spawnRadius);

            Gizmos.color = _hit.hit ? Color.blue : Color.red;

            Gizmos.DrawSphere(_hit.hit ? _hit.position : _point, 0.33f);
        }

        private void QuestStartHandler()
        {
            enabled = true;
        }

        private void EnemyDeathHandler(SpiderEnemyController enemy)
        {
            _healthService.RemoveCharacter(enemy.Health);
            _enemyPool.ReturnEnemy(enemy);
            OnEnemyDeath?.Invoke(_enemyPool.ActiveCount);
        }

        private void BossDeathHandler()
        {
            OnBossDeath?.Invoke();
            enabled = false;
        }
    }
}
