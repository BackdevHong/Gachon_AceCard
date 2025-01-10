using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingSceneManager : MonoBehaviour
{
    private int connectedPlayers = 0; // 접속한 플레이어 수
    public static WaitingSceneManager Instance;

    private void Awake()
    {
        // 싱글톤 패턴 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 서버를 연 플레이어가 접속
    public void HostConnected()
    {
        connectedPlayers = 1; // 첫 번째 플레이어
        Debug.Log("Host connected. Waiting for another player...");
    }

    // 다른 플레이어가 접속
    public void PlayerConnected()
    {
        connectedPlayers++;

        if (connectedPlayers >= 2)
        {
            Debug.Log("Both players connected. Moving to CardTestScene...");
            LoadNextScene();
        }
        else
        {
            Debug.Log("Waiting for more players...");
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("CardTestScene"); // CardTestScene으로 전환
    }
}