using UnityEngine;

public class DeckButton : MonoBehaviour
{
    public void OnDeckConfigButtonClick()
    {
        // TransitionManager�� ���� CardDeckScene���� ��ȯ
        TransitionManager.Instance.LoadSceneWithFade("CardDeckScene");
    }
}
