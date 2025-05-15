using UnityEngine;

namespace MainGame
{
    public class Quest
    {
        public QuestInfo QuestInfo;
        public QuestState QuestState;

        private int _currentQuestStepIndex;
        private QuestStepState[] _questStepStates;

        public Quest(QuestInfo info)
        {
            QuestInfo = info;
            QuestState = QuestState.RequirementNotMet;
            _currentQuestStepIndex = 0;
            _questStepStates = new QuestStepState[QuestInfo.QuestSteps.Length];
            for (int i = 0; i < _questStepStates.Length; i++)
            {
                _questStepStates[i] = new QuestStepState();
            }
        }

        public Quest(QuestInfo info, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
        {
            QuestInfo = info;
            QuestState = questState;
            _currentQuestStepIndex = currentQuestStepIndex;
            _questStepStates = questStepStates;

            if (_questStepStates.Length != QuestInfo.QuestSteps.Length)
            {
                Debug.LogWarning("QuestStepStates.Length != QuestInfo.QuestSteps.Length");
            }
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
                questStep.InitializeQuestStep(
                    QuestInfo.QuestId, 
                    _currentQuestStepIndex, 
                    _questStepStates[_currentQuestStepIndex].State
                    );
                
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

        public void StoreQuestStepState(QuestStepState questStepState, int questStepIndex)
        {
            if (questStepIndex < _questStepStates.Length)
            {
                _questStepStates[questStepIndex].State = questStepState.State;
            }
            else
            {
                Debug.LogWarning("Quest step doesn't exist");
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(QuestInfo.QuestId ,QuestState, _currentQuestStepIndex, _questStepStates);
        }
    }
}
