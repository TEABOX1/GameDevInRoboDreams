using System.Collections;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class EndGameScreen : BaseEndScreen
    {
        private QuestEvents _questEvents;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.GetService<QuestEvents>().OnFinishQuest += FinishQuestHandler;
        }

        protected override void Show()
        {
            base.Show();
            
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            YieldInstruction delay = new WaitForSeconds(_delay);
            
            yield return delay;

            float time = 0f;
            float reciprocal = 1f / _fadeDuration;

            while (time < _fadeDuration)
            {
                _canvasGroup.alpha = time * reciprocal;
                yield return null;
                time += Time.deltaTime;
            }
            _canvasGroup.alpha = 1f;
            
            yield return delay;
            
            time = 0f;
            while (time < _fadeDuration)
            {
                _menuButtonGroup.alpha = time * reciprocal;
                yield return null;
                time += Time.deltaTime;
            }
            _menuButtonGroup.alpha = 1f;

            _canvasGroup.interactable = true;
            _menuButtonGroup.interactable = true;
        }
        
        private void FinishQuestHandler(string questId)
        {
            if (questId == "KillBossQuest")
            {
                Show();
                
                _inputController.DefaulMapLock();
                _inputController.CursorEnable();
            }
        }
    }
}