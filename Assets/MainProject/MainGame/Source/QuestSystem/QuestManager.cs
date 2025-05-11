using System.Collections.Generic;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<string, Quest> _quests;

        private QuestEvents _questEvents;
        
        protected void Awake()
        {
            _quests = CreateQuestMap();
        }

        private void OnEnable()
        {
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();

            _questEvents.OnStartQuest += StartQuestHandler;
            _questEvents.OnAdvanceQuest += AdvanceQuestHandler;
            _questEvents.OnFinishQuest += FinishQuestHandler;
        }

        private void OnDisable()
        {
            _questEvents.OnStartQuest -= StartQuestHandler;
            _questEvents.OnAdvanceQuest -= AdvanceQuestHandler;
            _questEvents.OnFinishQuest -= FinishQuestHandler;
        }
        //TODO: Add save quests using this for load questSteps
        // private void Start()
        // {
        //     foreach (Quest quest in _quests.Values)
        //     {
        //         if (quest.QuestState == QuestState.InProgress)
        //         {
        //             quest.InstantiateCurrentQuestStep(transform);
        //         }
        //         _questEvents.QuestStateChange(quest);
        //     }
        // }

        private void Update()
        {
            foreach (Quest quest in _quests.Values)
            {
                if(quest.QuestState == QuestState.RequirementNotMet && CheckRequirements(quest))
                    ChangeQuestState(quest.QuestInfo.QuestId, QuestState.CanStart);
            }
        }
        
        private bool CheckRequirements(Quest quest)
        {
            bool metRequirements = true;

            foreach (QuestInfo prerequisiteQuestInfo in quest.QuestInfo.QuestPrerequisites)
            {
                if (GetQuestById(prerequisiteQuestInfo.QuestId).QuestState != QuestState.Finished)
                {
                    metRequirements = false;
                }
            }
            
            return metRequirements;
        }
        
        private void StartQuestHandler(string questId)
        {
            Quest quest = GetQuestById(questId);
            quest.InstantiateCurrentQuestStep(transform);
            ChangeQuestState(quest.QuestInfo.QuestId, QuestState.InProgress);
        }

        private void AdvanceQuestHandler(string questId)
        {
            Quest quest = GetQuestById(questId);
            
            quest.MoveToNextStep();
            if(quest.CurrentStepExists())
                quest.InstantiateCurrentQuestStep(transform);
            else
            {
                ChangeQuestState(quest.QuestInfo.QuestId, QuestState.CanFinish);
            }
        }

        private void FinishQuestHandler(string questId)
        {
            Quest quest = GetQuestById(questId);
            ClaimRewards(quest);
            ChangeQuestState(quest.QuestInfo.QuestId, QuestState.Finished);
        }
        
        private void ChangeQuestState(string id, QuestState state)
        {
            Quest quest = GetQuestById(id);
            quest.QuestState = state;
            _questEvents.QuestStateChange(quest);
        }

        private void ClaimRewards(Quest quest)
        {
            Debug.Log($"Claiming Rewards: {quest.QuestInfo.AbilityUnlockReward}");
            
            SpellData spellReward = quest.QuestInfo.AbilityUnlockReward;
            
            if (spellReward == null) return;
            SpellInventory spellInventory = ServiceLocator.Instance.GetService<SpellInventory>();
            spellInventory.UnlockSpell(spellReward);
        }
        
        private Dictionary<string, Quest> CreateQuestMap()
        {
            QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
            
            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

            for(int i = 0; i < allQuests.Length; i++)
            {
                QuestInfo info = allQuests[i];
                if(idToQuestMap.ContainsKey(info.QuestId))
                    Debug.Log($"Quest {info.QuestId} already exists");
                idToQuestMap.Add(info.QuestId, new Quest(info));
            }

            return idToQuestMap;
        }

        private Quest GetQuestById(string questId)
        {
            Quest quest = _quests[questId];
            if(quest == null)
                Debug.Log($"Quest {questId} not found");
            return quest;
        }
    }
}