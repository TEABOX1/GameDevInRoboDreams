using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [Serializable]
        public struct DialogueDataStruct
        {
            public string lineId;
            public string dialogueLine;
            public AnswerOption[] answerOption;
            public Sprite image;
        }
        
        [Serializable]
        public struct AnswerOption
        {
            public string answerText;
            public string nextLineId;
            public bool triggerComplete;
        }

        [SerializeField] private string _dialogueId;
        [SerializeField] private DialogueDataStruct[] _dialogueData;
        [Header("Alternative version of the dialogue for repetition")]
        [SerializeField] private Dialogue _alternativeDialogue;
        
        public string DialogueId => _dialogueId;
        public DialogueDataStruct[] DialogueData => _dialogueData;
        public Dialogue AlternativeDialogue => _alternativeDialogue;
        public bool WasPlayed = false;
        
        public DialogueDataStruct? GetLineById(string id)
        {
            foreach (var line in _dialogueData)
            {
                if (line.lineId == id)
                    return line;
            }

            return null;
        }
        
        private void OnValidate()
        {
            #if UNITY_EDITOR
            _dialogueId = name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}