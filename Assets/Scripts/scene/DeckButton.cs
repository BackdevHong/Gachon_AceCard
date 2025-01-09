using UnityEngine;

public class DeckButton : MonoBehaviour
{
    public void OnDeckConfigButtonClick()
    {
        // TransitionManager를 통해 CardDeckScene으로 전환
        TransitionManager.Instance.LoadSceneWithFade("CardDeckScene");
    }
}
