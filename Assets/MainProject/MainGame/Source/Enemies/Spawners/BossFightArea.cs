using GlobalSource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MainGame
{
    public class BossFightArea : HordeSpawner
    {
        public event Action<int> OnEnemyDeath;
        public event Action OnBossDeath;
        public event Action<IHealth> OnBossSpawn;

        [SerializeField] private NecroEnemyController _boss;
        [SerializeField] protected MapMarker _oldMarker;

        private EnemyService _enemyService;
        private QuestEvents _questEvents;
        private DialogueEvents _dialogEvents;
        //private IPlayerService _playerService;

        protected override void Awake()
        {
            enabled = false;

            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _dialogEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            //_playerService = ServiceLocator.Instance.GetService<IPlayerService>();

            //_questEvents.OnStartQuest += (questId) =>
            //{
            //    if (questId == "QuestInfo")
            //    {
            //        QuestStartHandler();
            //    }
            //};

            //Debug.Log("connect ro quest finish");
            //_questEvents.OnFinishQuest += (questId) =>
            //{
            //    if (questId == "KillSpidersQuest")
            //    {
            //        Debug.Log("try to handle quest");
            //        QuestStartHandler();
            //    }
            //};

            _dialogEvents.OnExitDialogue += ExitDialogueHandler; // підписка на закінчення діалогу з босом
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _oldMarker.gameObject.SetActive(false);
        }

        public Vector3 GetExactPoint(Vector3 targetPosition)
        {
            Debug.Log("try to get points");
            NavMesh.SamplePosition(targetPosition, out _hit, 10.0f, NavMesh.AllAreas);
            while (!_hit.hit)
            {
                NavMesh.SamplePosition(targetPosition, out _hit, 10.0f, NavMesh.AllAreas);
            }
            return _hit.position;
        }

        protected override void SpawnEnemy()
        {
            Debug.Log("spawn boss");
            Vector3 spawnPoint = GetExactPoint(_spawnPoint.position);
            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            var boss = Instantiate(_boss, spawnPoint, rotation);
            boss.Initialize(this);
            boss.enabled = false; // disable контролера боса для блокування атаки

            //_healthService.AddCharacter(boss.Health);
            boss.Health.OnDeath += () => BossDeathHandler(boss);
            _enemies.Add(boss);

            OnBossSpawn?.Invoke(boss.Health);
        }

        private void BossDeathHandler(NecroEnemyController boss)
        {
            OnBossDeath?.Invoke();
            _enemies.Remove(boss);
            List<Enemy> enemies = _enemyService.GetEnemies();
            foreach (var enemy in enemies)
            {
                enemy.Health.SetHealth(0);
            }
            //OnEnemyDeath?.Invoke(_enemies.Count);
            //enabled = false;
        }

        private void ExitDialogueHandler()
        {
            if (_enemies.Count == 0)
                return;
            _enemies[0].enabled = true; // enable контролера боса для атаки
        }
    }
}
