using UnityEngine;

public class MousePointerFollow : MonoBehaviour
{
    void Update()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z�� �� ���� (2D ȯ��)
        transform.position = mousePosition;
    }
}
