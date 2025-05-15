using UnityEngine;
using System.Collections;
using MainGame;

public class CalmAudioZoneTrigger : MonoBehaviour
{
    [SerializeField] public AudioSource _ambientAudioSource;
    [SerializeField] public PlayerController _playerController;
    [SerializeField] public AudioClip newClip;

    public float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other == _playerController.CharacterController)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeToNewClip(newClip));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _playerController.CharacterController)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeToNewClip(AudioClip clip)
    {
        yield return StartCoroutine(FadeOut());

        _ambientAudioSource.clip = clip;
        _ambientAudioSource.Play();

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = _ambientAudioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _ambientAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        _ambientAudioSource.volume = 0;
        _ambientAudioSource.Stop();
    }

    private IEnumerator FadeIn()
    {
        _ambientAudioSource.volume = 0;
        float targetVolume = 1f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _ambientAudioSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }

        _ambientAudioSource.volume = targetVolume;
    }
}