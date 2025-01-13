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
            DontDestroyOnLoad(Instance);
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
                HighlightObject(clickedObject, false); // 강조 해제
                Debug.Log($"{clickedObject.name} 선택이 취소되었습니다.");
            }
            else if (selectedObjects.Count < 3)
            {
                // 선택 목록에 추가
                selectedObjects.Add(clickedObject);
                HighlightObject(clickedObject, true); // 강조 추가
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
            List<GameObject> selectedPrefabs = new List<GameObject>();

            foreach (var selectedObject in selectedObjects)
            {
                var prefabRef = selectedObject.GetComponent<PrefabReference>();
                if (prefabRef != null && prefabRef.originalPrefab != null)
                {
                    selectedPrefabs.Add(prefabRef.originalPrefab);
                }
                else
                {
                    Debug.LogError($"{selectedObject.name}에 PrefabReference가 없거나 원본 프리팹이 null입니다.");
                }
            }

            if (selectedPrefabs.Count == 3)
            {
                GameManager.Instance.SetSelectedCards(selectedPrefabs);
                Debug.Log("프리팹이 성공적으로 저장되었습니다.");
            }
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
    private void HighlightObject(GameObject obj, bool isHighlighted)
    {
        if (isHighlighted)
        {
            obj.transform.localScale *= 1.1f; // 크기를 10% 증가
        }
        else
        {
            obj.transform.localScale /= 1.1f; // 원래 크기로 복원
        }
    }
}