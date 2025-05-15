using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MainGame
{
    // Запасний скрипт на випадок якщо бос і павуки будуть в одній локації
    public class Spawner : MonoBehaviour, INavPointProvider
    {
        //public event Action<int> OnEnemyDeath;
        public event Action OnBossSpawn;

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private Vector3 _offset;

        [SerializeField] private int _spawnCount = 3;

        [SerializeField] private NecroEnemyController _boss;
        [SerializeField] private SpiderEnemyController _enemy;
        //[SerializeField] private SpidersPool _enemyPool;

        private List<EnemyController> _enemies = new();

        private int priority;

        private Vector3 _point;
        private NavMeshHit _hit;

        private QuestEvents _questEvents;
        private DialogueEvents _dialogEvents;
        private IPlayerService _playerService;
        protected IHealthService _healthService;

        private void Awake()
        {
            enabled = false;

            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _dialogEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();

            _questEvents.OnStartQuest += (questId) =>
            {
                if (questId == "KillSpidersQuest")
                {
                    QuestStartHandler();
                }
            };

            _questEvents.OnStartQuest += (questId) =>
            {
                if (questId == "QuestInfo")
                {
                    SpawnBoss();
                }
            };
            // або якщо спочатку треба заспавнити боса і потім викликати діалог - підписатися на подію OnBossSpawn

            _dialogEvents.OnExitDialogue += ExitDialogueHandler;
        }

        private void OnEnable()
        {
            SpawnEnemies(_spawnCount);
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; ++i)
                SpawnEnemy();
        }

        private void SpawnEnemy()
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

            var enemy = Instantiate(_enemy, _hit.position, Quaternion.identity);
            //var enemy = _enemyPool.GetEnemy(_hit.position, Quaternion.identity);
            enemy.Initialize(this);

            enemy.NavMeshAgent.avoidancePriority = priority;
            priority++;

            _healthService.AddCharacter(enemy.Health);
            enemy.Health.OnDeath += () => EnemyDeathHandler(enemy);

            _enemies.Add(enemy);
        }

        private void SpawnBoss()
        {
            Vector3 spawnPoint = GetExactPoint(_spawnPoint.position);
            var boss = Instantiate(_boss, spawnPoint, _spawnPoint.rotation);
            boss.Initialize(this);

            _healthService.AddCharacter(boss.Health);
            boss.Health.OnDeath += () => BossDeathHandler();
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
            _enemies.Remove(enemy);
            //_enemyPool.ReturnEnemy(enemy);
            //OnEnemyDeath?.Invoke(EnemyCount);
        }

        private void BossDeathHandler()
        {
            enabled = false;
        }

        private void ExitDialogueHandler()
        {
            _playerService.Player.TargetPivot.gameObject.SetActive(true);
            _enemies[0].enabled = true; // треба вимкнути у префабі
        }
    }
}
