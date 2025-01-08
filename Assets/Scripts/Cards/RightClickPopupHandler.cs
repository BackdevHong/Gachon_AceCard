using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popup; // 카드 하위에 있는 팝업 오브젝트
    private static GameObject activePopup; // 현재 활성화된 팝업 (전역 관리)

    private void Start()
    {
        if (popup != null)
        {
            popup.SetActive(false); // 팝업 초기화 시 비활성화
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 카드 위에서 우클릭한 경우
                Debug.Log($"{gameObject.name} 카드가 우클릭되었습니다.");
                HandleRightClick();
            }
            else
            {
                // 빈 공간 또는 다른 UI를 우클릭한 경우
                CloseIfActive();
            }
        }
    }

    private void HandleRightClick()
    {
        // 다른 팝업이 활성화되어 있다면 비활성화
        if (activePopup != null && activePopup != popup)
        {
            activePopup.SetActive(false);
        }

        // 현재 카드의 팝업 활성화
        if (popup != null)
        {
            popup.SetActive(true);
            activePopup = popup; // 활성화된 팝업을 전역 변수에 저장
            Debug.Log($"{gameObject.name}의 팝업이 활성화되었습니다.");
        }
    }

    public void CloseIfActive()
    {
        // 현재 활성화된 팝업만 닫기
        if (activePopup == popup)
        {
            popup.SetActive(false);
            activePopup = null; // 전역 변수 초기화
            Debug.Log("활성화된 팝업이 닫혔습니다.");
        }
    }
}
