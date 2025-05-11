using UnityEngine;

namespace MainGame
{
    public class Quest
    {
        public QuestInfo QuestInfo;
        public QuestState QuestState;

        private int _currentQuestStepIndex;

        public Quest(QuestInfo info)
        {
            QuestInfo = info;
            QuestState = QuestState.RequirementNotMet;
            _currentQuestStepIndex = 0;
        }

        public void MoveToNextStep()
        {
            _currentQuestStepIndex++;
        }

        public bool CurrentStepExists()
        {
            return _currentQuestStepIndex < QuestInfo.QuestSteps.Length;
        }

        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            GameObject questStepPrefab = GetCurrentQuestState();
            if (questStepPrefab)
            {
                QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform)
                    .GetComponent<QuestStep>();
                questStep.InitializeQuestStep(QuestInfo.QuestId);
                
            }
        }

        private GameObject GetCurrentQuestState()
        {
            GameObject questStepPrefab = null;
            if (CurrentStepExists())
            {
                questStepPrefab = QuestInfo.QuestSteps[_currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("Quest step doesn't exist");
            }
            return questStepPrefab;
        }
    }
}
