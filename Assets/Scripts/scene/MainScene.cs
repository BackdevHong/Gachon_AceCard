using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void OnMainConfigButtonClick()
    {
        SceneManager.LoadScene("GameScene"); // CardDeckScene으로 전환
    }
}

