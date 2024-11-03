using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private static SceneController Instance;

    #region Singleton 
    public static SceneController instance
    {
        get
        {
            if (Instance == null)
            {
                var obj = FindObjectOfType<SceneController>();

                if (obj != null)
                {
                    Instance = obj;
                }
                else
                {
                    Instance = Create();
                }
            }
            return Instance;
        }
    }

    private static SceneController Create()
    {
        return Instantiate(Resources.Load<SceneController>("LoadingUI"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField]
    private CanvasGroup canvasGroup; // 알파값 조정

    [SerializeField]
    private Image progressBar_Fill; // 로딩바 이미지

    [SerializeField]
    private float fadeSpeed; // 페이드 속도

    private string loadSceneName;


    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        loadSceneName = sceneName;

        StartCoroutine(LoadSceneProcess());
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar_Fill.fillAmount = 0f;
        yield return StartCoroutine(Fade(true)); // Fade 코루틴이 끝날 때 까지 대기 

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if(op.progress < 0.9f)
            {
                progressBar_Fill.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar_Fill.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if(progressBar_Fill.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while(timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * fadeSpeed;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }

}
