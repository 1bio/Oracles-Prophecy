using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeInOut : MonoBehaviour
{
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
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
        Color[] colors = new Color[_renderers.Length];
        
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = _renderers[i].material.color;
            colors[i].a = startAlpha;
            _renderers[i].material.color = colors[i];
        }

        for (int t = 0; t <= duration; t++)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].a = alpha;
                _renderers[i].material.color = colors[i];
            }

            yield return null;
        }

        for (int i = 0; i < _renderers.Length; i++)
        {
            colors[i].a = endAlpha;
            _renderers[i].material.color = colors[i];
        }
    }
}
