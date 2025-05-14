using UnityEngine;

namespace MainGame
{
    [CreateAssetMenu(fileName = "QuestInfo", menuName = "Data/QuestInfo", order = 0)]
    public class QuestInfo : ScriptableObject
    {
        [Header("Quest Info")]
        [SerializeField] private string _questId;
        [SerializeField] private string _questName;
        // [SerializeField] private string _questDescription;
        [Header("Quest requirements")]
        [SerializeField] private QuestInfo[] _questPrerequisites;
        // [SerializeField] private int _playerLevelRequired;
        [Header("Quest steps")]
        [SerializeField] private GameObject[] _questSteps;
        [Header("Quest rewards")]
        [SerializeField] private SpellData _abilityUnlockReward;
        // [SerializeField] private int _experienceReward;
        // [SerializeField] private int _goldReward;
        
        public string QuestId => _questId;
        public string QuestName => _questName;
        // public string QuestDescription => _questDescription;
        public QuestInfo[] QuestPrerequisites => _questPrerequisites;
        public GameObject[] QuestSteps => _questSteps;
        public SpellData AbilityUnlockReward => _abilityUnlockReward;
        // public int ExperienceReward => _experienceReward;
        // public int GoldReward => _goldReward;

        private void OnValidate()
        {
            #if UNITY_EDITOR
            _questId = name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}