using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popupPrefab; // 카드별 팝업 프리팹
    private GameObject popupInstance;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"카드 {gameObject.name} 우클릭 감지");
                ShowPopup();
            }
        }
    }

    private void ShowPopup()
    {
        // 기존 팝업 제거
        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }

        // 팝업 프리팹이 설정되지 않았다면 오류 출력
        if (popupPrefab == null)
        {
            Debug.LogError($"팝업 프리팹이 {gameObject.name} 카드에 설정되지 않았습니다.");
            return;
        }

        // 팝업 생성
        popupInstance = Instantiate(popupPrefab, transform);

        // 팝업의 Canvas 설정을 현재 씬의 Canvas와 일치시킴
        Canvas popupCanvas = popupInstance.GetComponent<Canvas>();
        Canvas sceneCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (popupCanvas != null && sceneCanvas != null)
        {
            popupCanvas.renderMode = sceneCanvas.renderMode;
            popupCanvas.worldCamera = sceneCanvas.worldCamera;
            popupCanvas.sortingLayerID = sceneCanvas.sortingLayerID;
            popupCanvas.sortingOrder = sceneCanvas.sortingOrder;
        }

        // 팝업 위치 설정 (UI 좌표로 변환)
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

        // 팝업 활성화
        popupInstance.SetActive(true);
        Debug.Log($"카드 {gameObject.name}의 팝업이 생성되었습니다.");
    }


    public void ClosePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
            Debug.Log($"카드 {gameObject.name}의 팝업이 닫혔습니다.");
        }
    }
}
