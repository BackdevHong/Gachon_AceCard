using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckButton : MonoBehaviour
{
        public void OnDeckConfigButtonClick()
        {
            SceneManager.LoadScene("CardDeckScene"); // CardDeckScene���� ��ȯ
        }
}
