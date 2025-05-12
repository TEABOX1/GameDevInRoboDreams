using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool _isFinished = false;
        private string _questId;
        
        protected EnemyService _enemyService;
        protected QuestEvents _questEvents;
        
        protected virtual void OnEnable()
        {
            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
        }

        public void InitializeQuestStep(string questId)
        {
            _questId = questId;
        }
        
        protected void FinishQuestStep()
        {
            Debug.Log("Quest step finished");
            if (_isFinished) return;
            
            _isFinished = true;
                
            _questEvents.AdvanceQuest(_questId);
                
            Destroy(gameObject);
        }
    }
}
    