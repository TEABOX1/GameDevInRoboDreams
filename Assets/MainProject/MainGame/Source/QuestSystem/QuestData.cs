using System;

namespace MainGame
{
    [Serializable]
    public class QuestData
    {
        public string QuestId;
        public QuestState State;
        public int QuestStepIndex;
        public QuestStepState[] QuestStepStates;

        public QuestData(string questId, QuestState state, int questStepIndex, QuestStepState[] questStepStates)
        {
            QuestId = questId;
            State = state;
            QuestStepIndex = questStepIndex;
            QuestStepStates = questStepStates;
        }
    }
}