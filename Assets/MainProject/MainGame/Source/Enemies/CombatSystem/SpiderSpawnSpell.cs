using GlobalSource;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class SpiderSpawnSpell : MonoBehaviour
    {
        public event Action OnSpiderSpellCast; //added for animation
        public event Action<int> OnSpiderDeath;

        [SerializeField] private List<Transform> _areaPoints = new List<Transform>();
        [SerializeField] private float _spawnCooldown;
        [SerializeField] private NecroEnemyController _necroEnemyController;

        //[SerializeField] private SpidersPool _enemyPool;
        [SerializeField] private SpiderEnemyController _enemyController;
        [SerializeField] private NecroSpellCastAnimation necroSpellAnimation;

        private List<Vector3> _spawnPoints = new List<Vector3>();
        private List<EnemyController> _spiders = new List<EnemyController>();
        private SortedDictionary<float, EnemyController> _spidersCollection = new();

        private INavPointProvider _fightArea;
        private IPlayerService _playerService;
        //private IHealthService _healthService;

        //added for animation
        [ContextMenu("Force spawn Spiders")]
        private void ForceState()
        {
            SpawnSpiders();
        }
        //end of animation

        public float SpawnSpellCooldown => _spawnCooldown;
        public int SpawnSpidersCount => _spiders.Count;

        private void Awake()
        {
            //_healthService = ServiceLocator.Instance.GetService<IHealthService>();
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();
            _fightArea = _necroEnemyController.NavPointProvider;
            necroSpellAnimation.OnSpiderAnimationFinished += SpawnHandler;
        }

        private void Update()
        {
            _spidersCollection.Clear();
            for (int i = 0; i < _spiders.Count; i++)
            {
                float distance = (_spiders[i].NavMeshAgent.nextPosition - _playerService.Player.TargetPivot.position).magnitude;
                if (_spidersCollection.ContainsKey(distance))
                {
                    // Знаходимо максимальну дистанцію серед всіх записів з таким же ключем
                    float similarDistance = _spidersCollection.Keys.FirstOrDefault(k => Mathf.Approximately(k, distance));

                    distance = similarDistance + 0.01f;
                }
                _spidersCollection.Add(distance, _spiders[i]);
            }

            int priority = 1;

            foreach (KeyValuePair<float, EnemyController> entry in _spidersCollection)
            {
                entry.Value.NavMeshAgent.avoidancePriority = priority;
                entry.Value.NavMeshAgent.speed = 4.5f - priority;
                priority++;
            }
        }

        private void GetSpawnPoints()
        {
            _spawnPoints.Clear();
            Transform necromancerTransform = transform.parent;

            for (int i = 0; i < _areaPoints.Count; i++)
            {
                Vector3 point = necromancerTransform.TransformPoint(_areaPoints[i].localPosition);

                NavMeshHit hit;
                NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas);
                //int depth = 0;
                while (!hit.hit)
                {
                    NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas);
                    /*depth++;
                    if (depth > 100000)
                    {
                        Debug.LogError("Point sampling reached 100000 iterations, aborting");
                        return;
                    }*/
                }
                _spawnPoints.Add(hit.position);
            }
        }

        public void SpawnSpiders()
        {
            OnSpiderSpellCast?.Invoke(); // added for animation
            
        }

        protected void SpawnHandler()
        {
            Debug.Log("Necro Spawn Spiders");
            GetSpawnPoints();
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                Vector3 point = _spawnPoints[i];
                //var spider = _enemyPool.GetEnemy(point, _fightArea.transform.rotation);
                var spider = Instantiate(_enemyController, point, transform.rotation);
                spider.GetComponent<Enemy>().EnemyType = EnemyTypes.SpawnSpider;
                spider.Initialize(_fightArea);
                spider.NavMeshAgent.avoidancePriority = i; // виставлення пріоритетності

                //_healthService.AddCharacter(spider.Health);
                spider.Health.OnDeath += () => SpiderDeathHandler(spider);

                _spiders.Add(spider);
            }
        }

        private void SpiderDeathHandler(SpiderEnemyController spider)
        {
            //_healthService.RemoveCharacter(spider.Health);
            //_enemyPool.ReturnEnemy(spider);
            _spiders.Remove(spider);
            OnSpiderDeath?.Invoke(SpawnSpidersCount);
        }
    }
}