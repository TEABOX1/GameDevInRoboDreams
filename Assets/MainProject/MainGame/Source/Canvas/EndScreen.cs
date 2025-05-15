using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Boot;
using GlobalSource;

namespace MainGame
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private float _delay;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _loadButtonGroup;
        [SerializeField] private CanvasGroup _menuButtonGroup;
        [SerializeField] private bool _showOnWin;

        private void Awake()
        {
            _canvas.enabled = false;
            _menuButton.onClick.AddListener(MenuButtonHandler);
            _loadButton.onClick.AddListener(LoadButtonHandler);
            ServiceLocator.Instance.GetService<IPlayerService>().Player.Health.OnDeath += PlayerDeadHandler;
        }

        public void Show()
        {
            _canvas.enabled = true;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _loadButtonGroup.alpha = 0f;
            _loadButtonGroup.interactable = false;
            _menuButtonGroup.alpha = 0f;
            _menuButtonGroup.interactable = false;

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
        }
        
        private void MenuButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }

        private void LoadButtonHandler()
        {
            _canvas.enabled = false;
            ServiceLocator.Instance.GetService<ISaveService>().LoadAll();
        }
    }
}