using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckButton : MonoBehaviour
{
        public void OnDeckConfigButtonClick()
        {
            SceneManager.LoadScene("CardDeckScene"); // CardDeckScene으로 전환
            TransitionManager.Instance.LoadSceneWithFade("CardDeckScene");
    }
}
    