using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // FadePanel의 CanvasGroup
    public float fadeOutDuration = 1f; // 페이드 아웃 지속 시간
    public float fadeInDuration = 2f;  // 페이드 인 지속 시간 (밝아지는 속도)

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

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // 1. 페이드 아웃 (화면 어두워짐)
        yield return StartCoroutine(Fade(1f, fadeOutDuration)); // 페이드 아웃 속도 조절

        // 2. 씬 로드
        SceneManager.LoadScene(sceneName);

        // 3. 페이드 인 (화면 밝아짐)
        yield return StartCoroutine(Fade(0f, fadeInDuration)); // 페이드 인 속도 조절
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvasGroup.alpha; // 현재 Alpha 값
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha; // 정확히 목표 Alpha 값으로 설정
    }
}
