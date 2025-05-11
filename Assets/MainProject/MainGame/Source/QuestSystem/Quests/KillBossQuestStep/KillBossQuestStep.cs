namespace MainGame
{
    public class KillBossQuestStep : QuestStep
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Add subscription to event
            _enemyService.EnemyDied += BossKilled;
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
    }
}