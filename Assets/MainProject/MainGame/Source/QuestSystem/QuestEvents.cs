using System;
using GlobalSource;

namespace MainGame
{
    public class QuestEvents : MonoServiceBase
    {
        public override Type Type { get; } = typeof(QuestEvents);

        public event Action<string> OnStartQuest;
        public void StartQuest(string id) => OnStartQuest?.Invoke(id);

        public event Action<string> OnAdvanceQuest;
        public void AdvanceQuest(string id) => OnAdvanceQuest?.Invoke(id);

        public event Action<string> OnFinishQuest;
        public void FinishQuest(string id) => OnFinishQuest?.Invoke(id);

        public event Action<Quest> OnQuestStateChange;
        public void QuestStateChange(Quest quest) => OnQuestStateChange?.Invoke(quest);
        
        public event Action<string, int, QuestStepState> OnQuestStepStateChange;
        public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState) 
            => OnQuestStepStateChange?.Invoke(id, stepIndex, questStepState);

        public void ClearAllListeners()
        {
            OnStartQuest = null;
            OnAdvanceQuest = null;
            OnFinishQuest = null;
            OnQuestStateChange = null;
        }
    }
}