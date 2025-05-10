using Boot;
using GlobalSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscene
{
    public class CutSceneController : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private List<CanvasGroup> _cutsceneCanvases;
        [SerializeField] private float _fadeDuration = 1f;

        private int _currentIndex = 0;
        private bool _isFading = false;

        private void Awake()
        {
            _nextButton.onClick.AddListener(NextButtonHandler);

            foreach (var canvasGroup in _cutsceneCanvases)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            StartCoroutine(FadeCanvas(_cutsceneCanvases[0], true));
        }

        private void NextButtonHandler()
        {
            if (_isFading || _currentIndex >= _cutsceneCanvases.Count)
                return;

            StartCoroutine(SwitchToNextCanvas());
        }

        private IEnumerator SwitchToNextCanvas()
        {
            _isFading = true;

            yield return FadeCanvas(_cutsceneCanvases[_currentIndex], false);

            _currentIndex++;

            if (_currentIndex < _cutsceneCanvases.Count)
            {
                yield return FadeCanvas(_cutsceneCanvases[_currentIndex], true);
            }
            else
            {
                ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad += SceneLoadHandler;
                ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.Gameplay);
                _nextButton.interactable = false;
            }

            _isFading = false;
        }

        private IEnumerator FadeCanvas(CanvasGroup canvasGroup, bool fadeIn)
        {
            float start = fadeIn ? 0f : 1f;
            float end = fadeIn ? 1f : 0f;
            float elapsed = 0f;

            if (fadeIn)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            while (elapsed < _fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / _fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = end;

            if (!fadeIn)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        private void SceneLoadHandler(AsyncOperation asyncOperation)
        {
            ServiceLocator.Instance.GetService<ISceneManager>().onSceneLoad -= SceneLoadHandler;
        }
    }
}