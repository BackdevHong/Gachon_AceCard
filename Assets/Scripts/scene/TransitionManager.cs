using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // CanvasGroup으로 Alpha 값 제어
    public float fadeDuration = 1f; // 페이드 지속 시간

    public static TransitionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithFade(string SampleScene)
    {
        StartCoroutine(FadeAndLoadScene(SampleScene));
    }

    private IEnumerator FadeAndLoadScene(string SampleScene)
    {
        // 1. 화면을 어둡게 만듦 (페이드 아웃)
        yield return StartCoroutine(Fade(1f)); // 화면이 완전히 어두워질 때까지 기다림

        // 2. 씬 로드
        yield return StartCoroutine(LoadScene(SampleScene));

        // 3. 화면을 밝게 만듦 (페이드 인)
        yield return StartCoroutine(Fade(0f)); // 화면이 완전히 밝아짐
    }

    private IEnumerator LoadScene(string CardDeckScene)
    {
        // 씬을 비동기로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(CardDeckScene);
        asyncLoad.allowSceneActivation = false; // 씬 전환 대기

        // 씬 로드 진행 상태를 확인
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // 로드 완료 시
            {
                asyncLoad.allowSceneActivation = true; // 씬 활성화
            }
            yield return null;
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha; // 정확히 목표 alpha로 설정
    }
}
