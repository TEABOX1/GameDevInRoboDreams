using UnityEngine;
using UnityEngine.UI;
using GlobalSource;
using MainMenu;
using Boot;

namespace MainGame
{
    public class GameplayPauseMenu : MonoBehaviour
    {
        public enum MenuState
        {
            Hidden,
            Menu,
            Settings,
            Exit
        }

        [Space, Header("Canvases and menus")]
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Canvas _exitCanvas;
        [SerializeField] private SoundMenu _soundMenu;

        [Space, Header("Manu Buttons")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadLastSaveButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitToMainMenuButton;

        [Space, Header("Settings buttons")]
        [SerializeField] private Button _closeSettingsButton;

        [Space, Header("Exit buttons")]
        [SerializeField] private Button _confrimButton;
        [SerializeField] private Button _cancelButton;

        private InputController _inputController;

        private MenuState _menuState = MenuState.Hidden;
        private ISaveService _saveService;

        public bool Enabled
        {
            get => _mainCanvas.enabled;
            set
            {
                if (_mainCanvas.enabled == value)
                    return;
                _mainCanvas.enabled = value;
                ServiceLocator.Instance.GetService<IGameStateProvider>()?.SetGameState(value ? GameState.Paused : GameState.Gameplay);
            }
        }
        
        private void Awake()
        {
            //Menu buttons
            _continueButton.onClick.AddListener(ContinueGameButtonHandler);
            _saveButton.onClick.AddListener(SaveProgressHandler);
            _loadLastSaveButton.onClick.AddListener(LoadLastSaveButtonHandler);
            _settingsButton.onClick.AddListener(SettingsButtonHandler);
            _exitToMainMenuButton.onClick.AddListener(ExitToMainMenuButtonHandler);

            //Settings buttons
            _closeSettingsButton.onClick.AddListener(CloseSettingsButtonHandler);

            //Exit buttons
            _confrimButton.onClick.AddListener(ConfirmButtonHandler);
            _cancelButton.onClick.AddListener(CancelButtonHandler);
        }

        private void Start()
        {
            _mainCanvas.enabled = false;
            _exitCanvas.enabled = false;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _inputController.OnEscapeInput += EscapeHandler;
        }

        private void EscapeHandler()
        {
            switch (_menuState)
            {
                case MenuState.Hidden:
                    Enabled = true;
                    _menuState = MenuState.Menu;
                    break;
                case MenuState.Menu:
                    Enabled = false;
                    _menuState = MenuState.Hidden;
                    break;
                case MenuState.Settings:
                    CloseSettingsButtonHandler();
                    _menuState = MenuState.Menu;
                    break;
                case MenuState.Exit:
                    CancelButtonHandler();
                    _menuState = MenuState.Menu;
                    break;
            }
        }
       
        //Menu buttons
        private void ContinueGameButtonHandler()
        {
            Enabled = false;
            _menuState = MenuState.Hidden;
        }
        private void SaveProgressHandler()
        {
            _saveService.SaveAll();
        }

        private void LoadLastSaveButtonHandler()
        {
            _saveService.LoadAll();
        }

        private void SettingsButtonHandler()
        {
            _mainCanvas.enabled = false;
            _soundMenu.Enabled = true;
            _menuState = MenuState.Settings;
        }

        private void ExitToMainMenuButtonHandler()
        {
            _exitCanvas.enabled = true;
            _mainCanvas.enabled = false;
            _menuState = MenuState.Exit;
        }

        //Settings buttons
        private void CloseSettingsButtonHandler()
        {
            _mainCanvas.enabled = true;
            _soundMenu.Enabled = false;
            _menuState = MenuState.Menu;
            _saveService.SaveAll();
        }

        //Exit buttons
        private void ConfirmButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }

        private void CancelButtonHandler()
        {
            _exitCanvas.enabled = false;
            _mainCanvas.enabled = true;
            _menuState = MenuState.Menu;
        }
    }
}