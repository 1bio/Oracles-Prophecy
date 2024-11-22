using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private GameObject inGameUI; 

    [SerializeField] private float fadeTime = 1f; 

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);

        float elapsedTime = 0f; 
        Color color = fade.color;
        color.a = 1f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            fade.color = color;
            yield return null; 
        }

        color.a = 0f;
        fade.color = color;

        if(fade.color.a == 0f)
        {
            StartCoroutine(UIManager.instance.DisplayZoneName());
        }

        if (!inGameUI.activeSelf)
        {
            inGameUI.SetActive(true);
        }
    }

}
