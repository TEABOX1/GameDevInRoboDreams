using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool _isFinished = false;
        private string _questId;
        private int _questStepIndex;
        
        protected EnemyService _enemyService;
        protected QuestEvents _questEvents;
        
        protected virtual void OnEnable()
        {
            _enemyService = ServiceLocator.Instance.GetService<EnemyService>();
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
        }

        public void InitializeQuestStep(string questId, int questStepIndex, string questStepState)
        {
            _questId = questId;
            _questStepIndex = questStepIndex;
            if (!string.IsNullOrEmpty(questStepState))
            {
                SetQuestStepState(questStepState);
            }
        }
        
        protected void FinishQuestStep()
        {
            Debug.Log("Quest step finished");
            if (_isFinished) return;
            
            _isFinished = true;
                
            _questEvents.AdvanceQuest(_questId);
                
            Destroy(gameObject);
        }

        protected void ChangeQuestStepState(string newState)
        {
            _questEvents.QuestStepStateChange(_questId, _questStepIndex, new QuestStepState(newState));
        }
        
        protected abstract void SetQuestStepState(string newState);
    }
}
    