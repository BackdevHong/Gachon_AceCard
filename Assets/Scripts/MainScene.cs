using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void OnMainConfigButtonClick()
    {
        SceneManager.LoadScene("SampleScene"); // CardDeckScene���� ��ȯ
    }
}

