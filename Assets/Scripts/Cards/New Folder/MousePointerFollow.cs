using UnityEngine;

public class MousePointerFollow : MonoBehaviour
{
    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z축 값 고정 (2D 환경)
        transform.position = mousePosition;
    }
}
