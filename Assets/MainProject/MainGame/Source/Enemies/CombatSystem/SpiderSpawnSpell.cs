using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class SpiderSpawnSpell : MonoBehaviour
    {
        public event Action<int> OnSpiderDeath;

        [SerializeField] private List<Transform> _areaPoints = new List<Transform>();
        [SerializeField] private float _spawnCooldown;
        [SerializeField] private BossFightArea _fightArea;
        [SerializeField] private SpidersPool _enemyPool;

        private List<Vector3> _spawnPoints = new List<Vector3>();
        private IHealthService _healthService;

        public float SpawnSpellCooldown => _spawnCooldown;

        private void Awake()
        {
            GetSpawnPoints();
            _healthService = ServiceLocator.Instance.GetService<IHealthService>();
        }

        private void GetSpawnPoints()
        {
            for (int i = 0; i < _areaPoints.Count; i++)
            {
                Vector3 point = _areaPoints[i].position;
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
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                Vector3 point = _spawnPoints[i];
                var spider = _enemyPool.GetEnemy(point, _fightArea.transform.rotation);
                spider.Initialize(_fightArea);

                _healthService.AddCharacter(spider.Health);
                spider.Health.OnDeath += () => SpiderDeathHandler(spider);
            }  
        }

        private void SpiderDeathHandler(SpiderEnemyController spider)
        {
            _healthService.RemoveCharacter(spider.Health);
            _enemyPool.ReturnEnemy(spider);
            OnSpiderDeath?.Invoke(_enemyPool.ActiveCount);
        }
    }
}