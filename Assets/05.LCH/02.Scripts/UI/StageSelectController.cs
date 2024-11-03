using System.Collections;
using UnityEngine;

public class StageSelectController : MonoBehaviour
{
    public GameObject stageSelectWindow; 

    public CanvasGroup canvasGroup; // 알파 값 조정

    [SerializeField] private float fadeSpeed;


    private void OnTriggerEnter(Collider other)
    {
        stageSelectWindow.SetActive(true);

        StartCoroutine(Fade(true));
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(Fade(false));
    }

    #region Main Methods
    private IEnumerator Fade(bool isFadeIn)
    {
        float startAlpha = isFadeIn ? 0f : 1f; // 페이드 인 
        float endAlpha = isFadeIn ? 1f : 0f; // 페이드 아웃
        float timer = 0f;
     
        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer); // 페이드 처리
            yield return null;
        }

        // 페이드가 끝나고 창을 숨길 때만 비활성화
        if (!isFadeIn)
        {
            stageSelectWindow.SetActive(false);
        }
    }
    #endregion
}
