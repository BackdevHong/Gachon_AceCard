using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEventManager : MonoBehaviour
{
    private void OnEnable()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ��ư�� ã�� �̺�Ʈ ����
        Button yourButton = GameObject.Find("Deck Button")?.GetComponent<Button>();
        if (yourButton != null)
        {
            yourButton.onClick.RemoveAllListeners(); // ���� �̺�Ʈ ����
            yourButton.onClick.AddListener(() =>
            {
                // TransitionManager�� ���� �� ��ȯ ȣ��
                TransitionManager.Instance.LoadSceneWithFade("CardDeckScene");
            });
        }
        else
        {
            Debug.LogError("Button not found in the scene!");
        }
    }
}
