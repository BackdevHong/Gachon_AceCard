using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEventManager : MonoBehaviour
{
    private void OnEnable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 버튼을 찾아 이벤트 연결
        Button yourButton = GameObject.Find("Deck Button")?.GetComponent<Button>();
        if (yourButton != null)
        {
            yourButton.onClick.RemoveAllListeners(); // 기존 이벤트 제거
            yourButton.onClick.AddListener(() =>
            {
                // TransitionManager를 통해 씬 전환 호출
                TransitionManager.Instance.LoadSceneWithFade("CardDeckScene");
            });
        }
        else
        {
            Debug.LogError("Button not found in the scene!");
        }
    }
}
