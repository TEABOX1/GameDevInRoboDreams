using System.Collections;
using UnityEngine;

namespace Boot
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _companyCanvasGroup;
        [SerializeField] private CanvasGroup _gameNameCanvasGroup;
        [SerializeField] private float _fadeDuration = 1.5f;
        [SerializeField] private float _displayDuration = 2f;

        private IGameStateProvider _gameStateController;

        private void Start()
        {
            _gameStateController = ServiceLocator.Instance.GetService<IGameStateProvider>();
            StartCoroutine(ShowSequence());
        }

        private IEnumerator ShowSequence()
        {
            yield return StartCoroutine(FadeCanvas(_companyCanvasGroup, true));
            yield return new WaitForSeconds(_displayDuration);
            yield return StartCoroutine(FadeCanvas(_companyCanvasGroup, false));

            yield return StartCoroutine(FadeCanvas(_gameNameCanvasGroup, true));
            yield return new WaitForSeconds(_displayDuration);
            yield return StartCoroutine(FadeCanvas(_gameNameCanvasGroup, false));

            _gameStateController.SetGameState(GameState.MainMenu);
        }

        private IEnumerator FadeCanvas(CanvasGroup canvasGroup, bool fadeIn)
        {
            float start = fadeIn ? 0f : 1f;
            float end = fadeIn ? 1f : 0f;
            float elapsed = 0f;

            canvasGroup.alpha = start;
            canvasGroup.blocksRaycasts = fadeIn;
            canvasGroup.interactable = fadeIn;

            while (elapsed < _fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / _fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = end;
        }
    }
}
