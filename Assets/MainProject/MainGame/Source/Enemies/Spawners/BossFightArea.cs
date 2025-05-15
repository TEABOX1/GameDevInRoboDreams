using GlobalSource;
using System;
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

        private QuestEvents _questEvents;
        private DialogueEvents _dialogEvents;
        private IPlayerService _playerService;

        protected override void Awake()
        {
            enabled = false;

            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _dialogEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            _playerService = ServiceLocator.Instance.GetService<IPlayerService>();

            _questEvents.OnStartQuest += (questId) =>
            {
                if (questId == "QuestInfo")
                {
                    QuestStartHandler();
                }
            };

            // àáî ÿêùî ñïî÷àòêó òðåáà çàñïàâíèòè áîñà ³ ïîò³ì âèêëèêàòè ä³àëîã:
            /*_questEvents.OnFinishQuest += (questId) =>
            {
                if (questId == "KillSpidersQuest")
                {
                    QuestStartHandler();
                }
            };*/

            _dialogEvents.OnExitDialogue += ExitDialogueHandler;
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
            var boss = Instantiate(_boss, spawnPoint, _spawnPoint.rotation);
            boss.Initialize(this);

            _healthService.AddCharacter(boss.Health);
            boss.Health.OnDeath += () => BossDeathHandler(boss);
            _enemies.Add(boss);

            OnBossSpawn?.Invoke(boss.Health);
        }

        private void BossDeathHandler(NecroEnemyController boss)
        {
            _enemies.Remove(boss);
            //OnEnemyDeath?.Invoke(_enemies.Count);
            enabled = false;
        }

        private void ExitDialogueHandler()
        {
            _playerService.Player.TargetPivot.gameObject.SetActive(true);
            _enemies[0].enabled = true; // òðåáà âèìêíóòè ó ïðåôàá³
        }
    }
}
