using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<string, Quest> _quests;

        private QuestEvents _questEvents;
        private ISaveService _saveService;
        private GameplayPauseMenu _pauseMenu;
        private CheckpointService _checkpointService;
        protected void Awake()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _quests = CreateQuestMap();
        }

        private void OnEnable()
        {
            _questEvents = ServiceLocator.Instance.GetService<QuestEvents>();
            _pauseMenu = ServiceLocator.Instance.GetService<GameplayPauseMenu>();
            _checkpointService = ServiceLocator.Instance.GetService<CheckpointService>();
            _pauseMenu.OnSaveSignal += SaveQuests;
            _checkpointService.OnCheckpointReached += SaveQuests;
            //ServiceLocator.Instance.GetService<GameplayPauseMenu>().OnLoadSignal += LoadQuests;

            _questEvents.OnStartQuest += StartQuestHandler;
            _questEvents.OnAdvanceQuest += AdvanceQuestHandler;
            _questEvents.OnFinishQuest += FinishQuestHandler;
            _questEvents.OnQuestStepStateChange += QuestStepStateChangeHandler;
            
        }

        private void OnDisable()
        {
            _questEvents.OnStartQuest -= StartQuestHandler;
            _questEvents.OnAdvanceQuest -= AdvanceQuestHandler;
            _questEvents.OnFinishQuest -= FinishQuestHandler;
            _questEvents.OnQuestStepStateChange -= QuestStepStateChangeHandler;
            
            _pauseMenu.OnSaveSignal -= SaveQuests;
            _checkpointService.OnCheckpointReached -= SaveQuests;
        }
        
        private void Start()
        {
            foreach (Quest quest in _quests.Values)
            {
                if (quest.QuestState == QuestState.InProgress)
                {
                    quest.InstantiateCurrentQuestStep(transform);
                }
                _questEvents.QuestStateChange(quest);
                
                //TODO: Change to saving data instead of ClaimRewards every time
                if (quest.QuestState == QuestState.Finished)
                {
                    ClaimRewards(quest);
                }
            }
        }

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
            StartCoroutine(TimerForNextQuestStep(questId));
        }
        //TODO: Can be moved to another script
        private IEnumerator TimerForNextQuestStep(string questId)
        {
            yield return new WaitForSeconds(1f);
            
            Quest quest = GetQuestById(questId);
            
            quest.MoveToNextStep();
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(transform);
            }
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
            // SaveQuests();
        }

        private void ClaimRewards(Quest quest)
        {
            Debug.Log($"Claiming Rewards: {quest.QuestInfo.AbilityUnlockReward}");
            
            SpellData spellReward = quest.QuestInfo.AbilityUnlockReward;
            
            if (spellReward == null) return;
            SpellInventory spellInventory = ServiceLocator.Instance.GetService<SpellInventory>();
            spellInventory.UnlockSpell(spellReward);
        }

        private void QuestStepStateChangeHandler(string questId, int stepIndex, QuestStepState questStepState)
        {
           Quest quest = GetQuestById(questId);
           quest.StoreQuestStepState(questStepState, stepIndex);
           ChangeQuestState(questId, quest.QuestState);
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
                //idToQuestMap.Add(info.QuestId, new Quest(info));
                idToQuestMap.Add(info.QuestId, LoadQuests(info));
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

        // private void OnDestroy()
        // {
        //     SaveQuests();
        //
        //     Debug.Log("Saved Quests: ");
        //     foreach (QuestData questData in _saveService.SaveData.playerInfoData.questData)
        //     {
        //         Debug.Log(questData.QuestId);
        //         Debug.Log(questData.QuestStepIndex);
        //         Debug.Log(questData.State);
        //         foreach (var questStepState in questData.QuestStepStates)
        //         {
        //             Debug.Log(questStepState);
        //         }
        //         Debug.Log("\n");
        //     }
        //
        //     _saveService.SaveAll();
        // }
        
        private void SaveQuests()
        {
            Debug.Log("Quest saving");
            
            List<QuestData> allQuestsData = new List<QuestData>();

            foreach (Quest quest in _quests.Values)
            {
                if (quest.QuestState == QuestState.RequirementNotMet ||
                    quest.QuestState == QuestState.CanStart) continue;
                QuestData questData = quest.GetQuestData();
                allQuestsData.Add(questData);
            }
            _saveService.SaveData.playerInfoData.questData = allQuestsData.ToArray();
        }
        
        private Quest LoadQuests(QuestInfo questInfo)
        {
            Quest quest;
            QuestData[] questDataArray = _saveService.SaveData.playerInfoData.questData;

            if (questDataArray != null)
            {
                List<QuestData> savedQuests = questDataArray.ToList();
                if (savedQuests.Any(q => q.QuestId == questInfo.QuestId))
                {
                    QuestData savedQuestData = savedQuests
                        .First(q => q.QuestId == questInfo.QuestId);
                    quest = new Quest(
                        questInfo,
                        savedQuestData.State,
                        savedQuestData.QuestStepIndex,
                        savedQuestData.QuestStepStates
                    );
                    return quest;
                }
            }
            return new Quest(questInfo);
        }
    }
}