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
                Debug.Log($"{clickedObject.name} ������ ��ҵǾ����ϴ�.");
            }
            else if (selectedObjects.Count < 3)
            {
                // ���� ��Ͽ� �߰�
                selectedObjects.Add(clickedObject);
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
            Debug.Log("3���� ī�� ���� �Ϸ�");
            // �ٸ� �ý��ۿ� ���� �����͸� ���� ����
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
}
