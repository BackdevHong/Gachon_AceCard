using UnityEngine;

public class TransitionManagerInitializer : MonoBehaviour
{
    public GameObject transitionManagerPrefab; // TransitionManager ������

    private void Awake()
    {
        // TransitionManager �ν��Ͻ��� ���� ��� ����
        if (TransitionManager.Instance == null)
        {
            GameObject transitionManager = Instantiate(transitionManagerPrefab);
            transitionManager.SetActive(true); // TransitionManager�� Ȱ��ȭ
            Debug.Log("TransitionManagerInitializer: TransitionManager prefab instantiated.");
        }
    }
}
    