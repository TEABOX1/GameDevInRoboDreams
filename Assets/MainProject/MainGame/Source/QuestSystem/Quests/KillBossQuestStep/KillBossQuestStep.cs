using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class KillBossQuestStep : QuestStep
    {
        //TODO: Fix bug with movement when open dialogue
        //[SerializeField] private Dialogue _startFightDialogue;
        
        //private DialogueEvents _dialogueEvents;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Add subscription to event
            _enemyService.EnemyDied += BossKilled;
            //_dialogueEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            //_dialogueEvents.EnterDialogue(_startFightDialogue);
        }

        private void OnDisable()
        {
            //TODO: Add unsubscription from event
            _enemyService.EnemyDied -= BossKilled;
        }

        private void BossKilled(IEnemy enemy)
        {
            if (enemy.EnemyType != EnemyTypes.Boss) return;
            FinishQuestStep();
        }

        protected override void SetQuestStepState(string newState)
        {
        }
    }
}