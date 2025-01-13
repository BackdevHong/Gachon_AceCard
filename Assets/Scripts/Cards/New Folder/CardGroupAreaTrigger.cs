using UnityEngine;

public class CardGroupAreaTrigger : MonoBehaviour
{
    public CardHoverEffect cardGroupManager; // �׷� �Ŵ��� ����

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���콺 Ŀ���� �׷� ������ ������ ī�尡 �ö�
        if (other.CompareTag("MousePointer"))
        {
            cardGroupManager.OnMouseEnter();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // ���콺 Ŀ���� �׷� ������ ����� ī�尡 ������
        if (other.CompareTag("MousePointer"))
        {
            cardGroupManager.OnMouseExit();
        }
    }
}
