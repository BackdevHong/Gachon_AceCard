using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popup; // ī�� ������ �ִ� �˾� ������Ʈ
    private static GameObject activePopup; // ���� Ȱ��ȭ�� �˾� (���� ����)

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
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // ī�� ������ ��Ŭ���� ���
                Debug.Log($"{gameObject.name} ī�尡 ��Ŭ���Ǿ����ϴ�.");
                HandleRightClick();
            }
            else
            {
                // �� ���� �Ǵ� �ٸ� UI�� ��Ŭ���� ���
                CloseIfActive();
            }
        }
    }

    private void HandleRightClick()
    {
        // �ٸ� �˾��� Ȱ��ȭ�Ǿ� �ִٸ� ��Ȱ��ȭ
        if (activePopup != null && activePopup != popup)
        {
            activePopup.SetActive(false);
        }

        // ���� ī���� �˾� Ȱ��ȭ
        if (popup != null)
        {
            popup.SetActive(true);
            activePopup = popup; // Ȱ��ȭ�� �˾��� ���� ������ ����
            Debug.Log($"{gameObject.name}�� �˾��� Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    public void CloseIfActive()
    {
        // ���� Ȱ��ȭ�� �˾��� �ݱ�
        if (activePopup == popup)
        {
            popup.SetActive(false);
            activePopup = null; // ���� ���� �ʱ�ȭ
            Debug.Log("Ȱ��ȭ�� �˾��� �������ϴ�.");
        }
    }
}
