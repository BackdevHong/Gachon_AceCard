using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // CanvasGroup���� Alpha �� ����
    public float fadeDuration = 1f; // ���̵� ���� �ð�

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

    public void LoadSceneWithFade(string SampleScene)
    {
        StartCoroutine(FadeAndLoadScene(SampleScene));
    }

    private IEnumerator FadeAndLoadScene(string SampleScene)
    {
        // 1. ȭ���� ��Ӱ� ���� (���̵� �ƿ�)
        yield return StartCoroutine(Fade(1f)); // ȭ���� ������ ��ο��� ������ ��ٸ�

        // 2. �� �ε�
        yield return StartCoroutine(LoadScene(SampleScene));

        // 3. ȭ���� ��� ���� (���̵� ��)
        yield return StartCoroutine(Fade(0f)); // ȭ���� ������ �����
    }

    private IEnumerator LoadScene(string CardDeckScene)
    {
        // ���� �񵿱�� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(CardDeckScene);
        asyncLoad.allowSceneActivation = false; // �� ��ȯ ���

        // �� �ε� ���� ���¸� Ȯ��
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) // �ε� �Ϸ� ��
            {
                asyncLoad.allowSceneActivation = true; // �� Ȱ��ȭ
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

        fadeCanvasGroup.alpha = targetAlpha; // ��Ȯ�� ��ǥ alpha�� ����
    }
}
