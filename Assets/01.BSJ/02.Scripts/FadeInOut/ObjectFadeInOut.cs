using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFadeInOut : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    public void StartFadeInOut()
    {
        StartCoroutine(FadeSequence(5));
    }

    private IEnumerator FadeSequence(float duration)
    {
        yield return StartCoroutine(FadeInOut(duration / 2, 0, 1));
        yield return StartCoroutine(FadeInOut(duration / 2, 1, 0));
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
            Debug.Log($"Alpha: {_fadeImage.color.a}");

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        _fadeImage.color = color;
    }
}
