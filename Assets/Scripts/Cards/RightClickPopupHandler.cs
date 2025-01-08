using UnityEngine;

public class RightClickPopupHandler : MonoBehaviour
{
    public GameObject popup; // 팝업 오브젝트
    private static GameObject activePopup;

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
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("우클릭 감지 (2D)");
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
            Debug.Log("우클릭으로 팝업이 활성화되었습니다.");
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
