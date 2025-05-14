using System;
using GlobalSource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class DialogueUI : MonoBehaviour
    {
        [Serializable]
        public struct AnswerButton
        {
            public Button button;
            public TMP_Text text;
        }
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private Image _characterImage;
        [SerializeField] private AnswerButton[] _answerButtons;
        
        private DialogueEvents _dialogueEvents;

        private Dialogue _currentDialogue;
        
        private void OnEnable()
        {
            _dialogueEvents = ServiceLocator.Instance.GetService<DialogueEvents>();
            _dialogueEvents.OnCheckDialogue += EnterDialogueHandler;
            _dialogueEvents.OnExitDialogue += ExitDialogueHandler;
            _dialogueEvents.OnLineUpdated += LineUpdatedHandler;
        }
        
        private void OnDisable()
        {
            _dialogueEvents.OnCheckDialogue -= EnterDialogueHandler;
            _dialogueEvents.OnExitDialogue -= ExitDialogueHandler;
            _dialogueEvents.OnLineUpdated -= LineUpdatedHandler;
        }

        private void EnterDialogueHandler(Dialogue dialogue)
        {
            _canvas.enabled = true;

            _currentDialogue = dialogue;
            
            var firstLine = dialogue.DialogueData[0];
            ChangeDialogueLine(firstLine);
        }

        private void LineUpdatedHandler(string lineId)
        {
            var line = _currentDialogue.GetLineById(lineId);

            if (line == null) return;
            ChangeDialogueLine(line.Value);
        }
        
        private void ChangeDialogueLine(Dialogue.DialogueDataStruct line)
        {
            _dialogueText.text = line.dialogueLine;
            _characterImage.sprite = line.image;
            
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                bool hasAnswer = i < line.answerOption.Length;
                _answerButtons[i].button.gameObject.SetActive(hasAnswer);

                if (!hasAnswer) continue;
                
                var option = line.answerOption[i];
                _answerButtons[i].text.text = option.answerText;
                
                var button = _answerButtons[i].button;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                    _dialogueEvents.SelectAnswer(option.answerText));
            }
        }
        
        private void ExitDialogueHandler()
        {
            _canvas.enabled = false;
        }
    }
}