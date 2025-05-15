using Boot;
using GlobalSource;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Cutscene
{
    public class CutSceneController : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private CanvasGroup _fadeOverlay;
        [SerializeField] private float _fadeDuration = 1f;

        private bool _isSceneLoading = false;

        private void Awake()
        {
            _nextButton.onClick.AddListener(SkipVideoHandler);

            if (_fadeOverlay != null)
            {
                _fadeOverlay.alpha = 0f;
                _fadeOverlay.blocksRaycasts = false;
            }

            if (_videoPlayer != null)
            {
                _videoPlayer.loopPointReached += OnVideoEnd;
                _videoPlayer.Play();
            }
        }

        private void SkipVideoHandler()
        {
            if (_isSceneLoading)
                return;

            StartCoroutine(FadeAndLoadNextScene());
        }

        private void OnVideoEnd(VideoPlayer vp)
        {
            if (_isSceneLoading)
                return;

            StartCoroutine(FadeAndLoadNextScene());
        }

        private IEnumerator FadeAndLoadNextScene()
        {
            _isSceneLoading = true;
            _nextButton.interactable = false;

            if (_fadeOverlay != null)
            {
                _fadeOverlay.blocksRaycasts = true;
                float elapsed = 0f;

                while (elapsed < _fadeDuration)
                {
                    _fadeOverlay.alpha = Mathf.Lerp(0f, 1f, elapsed / _fadeDuration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                _fadeOverlay.alpha = 1f;
            }

            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.Gameplay);
        }

        private void SceneLoadHandler(AsyncOperation asyncOperation)
        {
            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad -= SceneLoadHandler;
        }
    }
}