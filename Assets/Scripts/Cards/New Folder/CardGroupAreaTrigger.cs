using UnityEngine;

public class CardGroupAreaTrigger : MonoBehaviour
{
    public CardHoverEffect cardGroupManager; // 그룹 매니저 연결

    void OnTriggerEnter2D(Collider2D other)
    {
        // 마우스 커서가 그룹 영역에 들어오면 카드가 올라감
        if (other.CompareTag("MousePointer"))
        {
            cardGroupManager.OnMouseEnter();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 마우스 커서가 그룹 영역을 벗어나면 카드가 내려감
        if (other.CompareTag("MousePointer"))
        {
            cardGroupManager.OnMouseExit();
        }
    }
}
