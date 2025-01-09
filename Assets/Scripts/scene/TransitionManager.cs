using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // FadePanel�� CanvasGroup
    public float fadeOutDuration = 1f; // ���̵� �ƿ� ���� �ð�
    public float fadeInDuration = 2f;  // ���̵� �� ���� �ð� (������� �ӵ�)

    public static TransitionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �������� ����
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
        // 1. ���̵� �ƿ� (ȭ�� ��ο���)
        yield return StartCoroutine(Fade(1f, fadeOutDuration)); // ���̵� �ƿ� �ӵ� ����

        // 2. �� �ε�
        SceneManager.LoadScene(sceneName);

        // 3. ���̵� �� (ȭ�� �����)
        yield return StartCoroutine(Fade(0f, fadeInDuration)); // ���̵� �� �ӵ� ����
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvasGroup.alpha; // ���� Alpha ��
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha; // ��Ȯ�� ��ǥ Alpha ������ ����
    }
}
