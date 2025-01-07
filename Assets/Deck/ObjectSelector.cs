using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public List<GameObject> selectableObjects; // 선택 가능한 오브젝트 리스트
    private List<GameObject> selectedObjects = new List<GameObject>(); // 선택된 오브젝트 저장
    public static ObjectSelector Instance;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectObject(GameObject clickedObject)
    {
        // 클릭된 오브젝트가 선택 가능한 오브젝트인지 확인
        if (selectableObjects.Contains(clickedObject))
        {
            // 이미 선택된 오브젝트인지 확인
            if (selectedObjects.Contains(clickedObject))
            {
                // 선택 취소
                selectedObjects.Remove(clickedObject);
                Debug.Log($"{clickedObject.name} 선택이 취소되었습니다.");
            }
            else if (selectedObjects.Count < 3)
            {
                // 선택 목록에 추가
                selectedObjects.Add(clickedObject);
                Debug.Log($"오브젝트 셀렉터{clickedObject.name}이(가) 선택되었습니다.");
            }
            else
            {
                Debug.Log("3개의 오브젝트만 선택할 수 있습니다.");
            }
        }
        else
        {
            Debug.Log("선택할 수 없는 오브젝트입니다.");
        }
    }

    public void ConfirmSelection()
    {
        if (selectedObjects.Count == 3)
        {
            Debug.Log("3개의 카드 선택 완료");
            // 다른 시스템에 선택 데이터를 전달 가능
        }
        else
        {
            Debug.Log("3개의 오브젝트를 모두 선택해야 합니다.");
        }
    }

    public List<GameObject> GetSelectedObjects()
    {
        return new List<GameObject>(selectedObjects);
    }
}
