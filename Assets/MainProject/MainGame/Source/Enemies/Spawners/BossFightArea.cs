using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class BossFightArea : HordeSpawner
    {
        public event Action<int> OnEnemyDeath;
        public event Action OnBossDeath;

        [SerializeField] private NecroEnemyController _boss;
        //[SerializeField] private SpidersPatrolArea _questArea;

        protected override void Awake()
        {
            enabled = false;
            //Підписка на подію старту квесту
            //_questArea.OnQuestCompleted += QuestStartHandler;
        }

        public Vector3 GetExactPoint(Vector3 targetPosition)
        {
            NavMesh.SamplePosition(targetPosition, out _hit, 1.0f, NavMesh.AllAreas);
            while (!_hit.hit)
            {
                NavMesh.SamplePosition(targetPosition, out _hit, 1.0f, NavMesh.AllAreas);
            }
            return _hit.position;
        }

        protected override void SpawnEnemy()
        {
            Vector3 spawnPoint = GetExactPoint(_spawnPoint.position);
            NecroEnemyController boss = Instantiate(_boss, spawnPoint, _spawnPoint.rotation);
            boss.Initialize(this);

            _healthService.AddCharacter(boss.Health);
            boss.Health.OnDeath += () => BossDeathHandler(boss);
            _enemies.Add(boss);
        }

        private void BossDeathHandler(NecroEnemyController boss)
        {
            _enemies.Remove(boss);
            OnEnemyDeath?.Invoke(_enemies.Count);
            OnBossDeath?.Invoke();
        }
    }
}
