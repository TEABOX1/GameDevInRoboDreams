using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GlobalSource;
using Boot;
using UnityEngine.EventSystems;

namespace MainMenu
{
    /// </summary>
    public class LobbyMainMenu : MonoBehaviour
    {
        [Serializable]
        public struct MenuAnimationData
        {
            public Vector2 closedPosition;
            public Vector2 openPosition;
        }
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _menuTransform;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _closeSettingsButton;
        [SerializeField] private SoundMenu _soundMenu;
        [SerializeField] private RectTransform _settingsTransform;

        private Coroutine _fadeRoutine;
        private ISaveService _saveService;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(NewGameButtonHandler);
            _loadGameButton.onClick.AddListener(LoadGameButtonHandler);
            _quitButton.onClick.AddListener(QuitButtonHandler);
            _settingsButton.onClick.AddListener(SettingsButtonHandler);
            _closeSettingsButton.onClick.AddListener(CloseSettingsButtonHandler);

            _canvas.enabled = true;
            _soundMenu.Enabled = false;

            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
        }

        private void NewGameButtonHandler()
        {
            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.Cutscene);

            _saveService.SaveData.playerInfoData.DebugSaveInfo = 0;
        }

        private void LoadGameButtonHandler()
        {
            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.Gameplay);
        }

        private void QuitButtonHandler()
        {
#if UNITY_EDITOR
            _saveService.SaveAll();
            EditorApplication.isPlaying = false;
#else
            _saveService.SaveAll();
            Application.Quit();
#endif
        }

        private void SceneLoadHandler(AsyncOperation asyncOperation)
        {
            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad -= SceneLoadHandler;
        }

        private void SettingsButtonHandler()
        {
            _soundMenu.Enabled = true;
            _canvas.enabled = false;
        }

        private void CloseSettingsButtonHandler()
        {
            _canvas.enabled = true;
            _soundMenu.Enabled = false;

            _saveService.SaveAll();
        }
    }
}