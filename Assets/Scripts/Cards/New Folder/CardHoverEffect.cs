using UnityEngine;

public class CardHoverEffect : MonoBehaviour
{
    public GameObject cardGroupParent; // 카드 그룹의 부모 오브젝트
    public float hoverHeight = 30f;   // 카드 그룹이 올라가는 높이
    public float moveSpeed = 5f;      // 이동 속도

    private Vector3 originalPosition; // 부모 오브젝트의 원래 위치 저장
    private Vector3 targetPosition;   // 부모 오브젝트의 목표 위치
    private bool isHovering = false;  // 카드가 올라가 있는지 여부

    void Start()
    {
        // 부모 오브젝트의 초기 위치를 저장
        originalPosition = cardGroupParent.transform.position;
        targetPosition = originalPosition; // 초기 목표 위치는 현재 위치와 동일
    }

    void Update()
    {
        // 부모 오브젝트를 목표 위치로 부드럽게 이동
        cardGroupParent.transform.position = Vector3.Lerp(
            cardGroupParent.transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }

    public void OnMouseEnter()
    {
        // 마우스가 들어왔을 때 목표 위치를 위로 설정
        targetPosition = originalPosition + new Vector3(0, hoverHeight, 0);
        isHovering = true;
    }

    public void OnMouseExit()
    {
        // 마우스가 나갔을 때 목표 위치를 원래 위치로 설정
        targetPosition = originalPosition;
        isHovering = false;
    }
}
