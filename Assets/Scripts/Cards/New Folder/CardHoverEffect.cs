using UnityEngine;

public class CardHoverEffect : MonoBehaviour
{
    public GameObject cardGroupParent; // ī�� �׷��� �θ� ������Ʈ
    public float hoverHeight = 30f;   // ī�� �׷��� �ö󰡴� ����
    public float moveSpeed = 5f;      // �̵� �ӵ�

    private Vector3 originalPosition; // �θ� ������Ʈ�� ���� ��ġ ����
    private Vector3 targetPosition;   // �θ� ������Ʈ�� ��ǥ ��ġ
    private bool isHovering = false;  // ī�尡 �ö� �ִ��� ����

    void Start()
    {
        // �θ� ������Ʈ�� �ʱ� ��ġ�� ����
        originalPosition = cardGroupParent.transform.position;
        targetPosition = originalPosition; // �ʱ� ��ǥ ��ġ�� ���� ��ġ�� ����
    }

    void Update()
    {
        // �θ� ������Ʈ�� ��ǥ ��ġ�� �ε巴�� �̵�
        cardGroupParent.transform.position = Vector3.Lerp(
            cardGroupParent.transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }

    public void OnMouseEnter()
    {
        // ���콺�� ������ �� ��ǥ ��ġ�� ���� ����
        targetPosition = originalPosition + new Vector3(0, hoverHeight, 0);
        isHovering = true;
    }

    public void OnMouseExit()
    {
        // ���콺�� ������ �� ��ǥ ��ġ�� ���� ��ġ�� ����
        targetPosition = originalPosition;
        isHovering = false;
    }
}
