using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFadeInOut : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    public void StartFadeInOut(float duration, float startAlpha, float endAlpha)
    {
        StartCoroutine(FadeSequence(duration, startAlpha, endAlpha));
    }

    private IEnumerator FadeSequence(float duration, float startAlpha, float endAlpha)
    {
        yield return StartCoroutine(FadeInOut(duration / 2, startAlpha, endAlpha));
        yield return StartCoroutine(FadeInOut(duration / 2, endAlpha, startAlpha));
        StopAllCoroutines();
    }

    public void StartFadeIn(float duration)
    {
        StartCoroutine(FadeInOut(duration, 0, 1));
    }

    public void StartFadeOut(float duration)
    {
        StartCoroutine(FadeInOut(duration, 1, 0));
    }

    private IEnumerator FadeInOut(float duration, float startAlpha, float endAlpha)
    {
        Color color = _fadeImage.color;
        color.a = startAlpha;
        _fadeImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            _fadeImage.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        _fadeImage.color = color;
    }
}
