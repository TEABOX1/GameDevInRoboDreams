using UnityEngine;

namespace MainGame.DestroyEggsQuestStep
{
    public class DestroyEggsQuestStep : QuestStep
    {
        [SerializeField] private int _eggsToDestroy = 10;
        private int _eggsDestroyed = 0;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Add subscription to event
            _enemyService.EnemyDied += EggDestroyed;
        }

        private void OnDisable()
        {
            //TODO: Add unsubscription from event
            _enemyService.EnemyDied -= EggDestroyed;
        }

        private void EggDestroyed(IEnemy enemy)
        {
            if (enemy.EnemyType != EnemyTypes.Egg) return;

            _eggsDestroyed++;
            UpdateState();
            
            if (_eggsDestroyed >= _eggsToDestroy)
                FinishQuestStep();
        }
        
        private void UpdateState()
        {
            string state = _eggsDestroyed.ToString();
            ChangeQuestStepState(state);
        }

        protected override void SetQuestStepState(string newState)
        {
            _eggsDestroyed = int.Parse(newState);
            UpdateState();
        }
    }
}