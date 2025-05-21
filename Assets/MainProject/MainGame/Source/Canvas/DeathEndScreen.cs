using System.Collections;
using Boot;
using GlobalSource;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class DeathEndScreen : BaseEndScreen
    {
        [SerializeField] private Button _loadButton;
        [SerializeField] private CanvasGroup _loadButtonGroup;

        protected override void Awake()
        {
            base.Awake();
            _loadButton.onClick.AddListener(LoadButtonHandler);
            ServiceLocator.Instance.GetService<IPlayerService>().Player.Health.OnDeath += PlayerDeadHandler;
        }
        
        protected override void Show()
        {
            base.Show();
            
            _loadButtonGroup.alpha = 0f;
            _loadButtonGroup.interactable = false;
            
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
                _loadButtonGroup.alpha = time * reciprocal;
                _menuButtonGroup.alpha = time * reciprocal;
                yield return null;
                time += Time.deltaTime;
            }
            _loadButtonGroup.alpha = 1f;
            _menuButtonGroup.alpha = 1f;

            _canvasGroup.interactable = true;
            _loadButtonGroup.interactable = true;
            _menuButtonGroup.interactable = true;
        }
        
        private void PlayerDeadHandler()
        {
            Show();
            _inputController.DefaulMapLock();
            _inputController.CursorEnable();
        }
        
        private void LoadButtonHandler()
        {
            _canvas.enabled = false;
            // ServiceLocator.Instance.GetService<ISaveService>().LoadAll();
            ServiceLocator.Instance.GetService<ISceneManager>().ReloadCurrentScene();
        }
    }
}