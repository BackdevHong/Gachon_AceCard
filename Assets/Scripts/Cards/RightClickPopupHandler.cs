using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popupPrefab; // ī�庰 �˾� ������
    private GameObject popupInstance;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ��Ŭ��
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"ī�� {gameObject.name} ��Ŭ�� ����");
                ShowPopup();
            }
        }
    }

    private void ShowPopup()
    {
        // ���� �˾� ����
        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }

        // �˾� �������� �������� �ʾҴٸ� ���� ���
        if (popupPrefab == null)
        {
            Debug.LogError($"�˾� �������� {gameObject.name} ī�忡 �������� �ʾҽ��ϴ�.");
            return;
        }

        // �˾� ����
        popupInstance = Instantiate(popupPrefab, transform);

        // �˾��� Canvas ������ ���� ���� Canvas�� ��ġ��Ŵ
        Canvas popupCanvas = popupInstance.GetComponent<Canvas>();
        Canvas sceneCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (popupCanvas != null && sceneCanvas != null)
        {
            popupCanvas.renderMode = sceneCanvas.renderMode;
            popupCanvas.worldCamera = sceneCanvas.worldCamera;
            popupCanvas.sortingLayerID = sceneCanvas.sortingLayerID;
            popupCanvas.sortingOrder = sceneCanvas.sortingOrder;
        }

        // �˾� ��ġ ���� (UI ��ǥ�� ��ȯ)
        RectTransform popupRect = popupInstance.GetComponent<RectTransform>();
        if (popupRect != null)
        {
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                sceneCanvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                sceneCanvas.GetComponent<Canvas>().worldCamera,
                out anchoredPosition
            );
            popupRect.anchoredPosition = anchoredPosition;
        }

        // �˾� Ȱ��ȭ
        popupInstance.SetActive(true);
        Debug.Log($"ī�� {gameObject.name}�� �˾��� �����Ǿ����ϴ�.");
    }


    public void ClosePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
            Debug.Log($"ī�� {gameObject.name}�� �˾��� �������ϴ�.");
        }
    }
}
