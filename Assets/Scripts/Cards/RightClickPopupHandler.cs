using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popup; // �˾� ������Ʈ
    private static GameObject activePopup;

    private void Start()
    {
        if (popup != null)
        {
            popup.SetActive(false); // �˾� �ʱ�ȭ �� ��Ȱ��ȭ
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ��Ŭ��
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("��Ŭ�� ���� (2D)");
                HandleRightClick();
            }
        }
    }

    private void HandleRightClick()
    {
        if (activePopup != null && activePopup != popup)
        {
            activePopup.SetActive(false);
        }

        if (popup != null)
        {
            popup.SetActive(true);
            activePopup = popup;
            Debug.Log("��Ŭ������ �˾��� Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    public void ClosePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
            if (activePopup == popup)
            {
                activePopup = null;
            }
        }
    }
}
