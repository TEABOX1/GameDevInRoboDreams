using System;
using UnityEngine;

namespace MainGame
{
    public class KillSpidersQuestStep : QuestStep
    {
        [SerializeField] private int _spidersToKill = 3;
        private int _spidersKilled = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Add subscription to event
            _enemyService.EnemyDied += SpiderKilled;
        }

        private void OnDisable()
        {
            //TODO: Add unsubscription from event
            _enemyService.EnemyDied -= SpiderKilled;
        }

        private void SpiderKilled(IEnemy enemy)
        {
            if (enemy.EnemyType != EnemyTypes.Spider) return;
            
            _spidersKilled++;
            UpdateState();
            
            if (_spidersKilled >= _spidersToKill)
            {
                FinishQuestStep();
            }
        }

        private void UpdateState()
        {
            string state = _spidersKilled.ToString();
            ChangeQuestStepState(state);
        }

        protected override void SetQuestStepState(string newState)
        {
            _spidersKilled = int.Parse(newState);
            UpdateState();
        }
    }
}