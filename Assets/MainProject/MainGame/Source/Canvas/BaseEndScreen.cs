using Boot;
using GlobalSource;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class BaseEndScreen : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected Button _menuButton;
        [SerializeField] protected float _delay = 2f;
        [SerializeField] protected float _fadeDuration = 0.5f;
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected CanvasGroup _menuButtonGroup;

        protected InputController _inputController;

        protected virtual void Awake()
        {
            _canvas.enabled = false;
            _menuButton.onClick.AddListener(MenuButtonHandler);
            _inputController = ServiceLocator.Instance.GetService<InputController>();
        }

        protected virtual void Show()
        {
            _canvas.enabled = true;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _menuButtonGroup.alpha = 0f;
            _menuButtonGroup.interactable = false;
        }

        private void MenuButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }
    }
}