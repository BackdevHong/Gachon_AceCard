using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public List<GameObject> selectableObjects; // ���� ������ ������Ʈ ����Ʈ
    private List<GameObject> selectedObjects = new List<GameObject>(); // ���õ� ������Ʈ ����
    public static ObjectSelector Instance;

    private void Awake()
    {
        // �̱��� ����
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
        // Ŭ���� ������Ʈ�� ���� ������ ������Ʈ���� Ȯ��
        if (selectableObjects.Contains(clickedObject))
        {
            // �̹� ���õ� ������Ʈ���� Ȯ��
            if (selectedObjects.Contains(clickedObject))
            {
                // ���� ���
                selectedObjects.Remove(clickedObject);
                HighlightObject(clickedObject, false); // ���� ����
                Debug.Log($"{clickedObject.name} ������ ��ҵǾ����ϴ�.");
            }
            else if (selectedObjects.Count < 3)
            {
                // ���� ��Ͽ� �߰�
                selectedObjects.Add(clickedObject);
                HighlightObject(clickedObject, true); // ���� �߰�
                Debug.Log($"������Ʈ ������{clickedObject.name}��(��) ���õǾ����ϴ�.");
            }
            else
            {
                Debug.Log("3���� ������Ʈ�� ������ �� �ֽ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("������ �� ���� ������Ʈ�Դϴ�.");
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
                    Debug.LogError($"{selectedObject.name}�� PrefabReference�� ���ų� ���� �������� null�Դϴ�.");
                }
            }

            if (selectedPrefabs.Count == 3)
            {
                GameManager.Instance.SetSelectedCards(selectedPrefabs);
                Debug.Log("�������� ���������� ����Ǿ����ϴ�.");
            }
        }
        else
        {
            Debug.Log("3���� ������Ʈ�� ��� �����ؾ� �մϴ�.");
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
            obj.transform.localScale *= 1.1f; // ũ�⸦ 10% ����
        }
        else
        {
            obj.transform.localScale /= 1.1f; // ���� ũ��� ����
        }
    }
}