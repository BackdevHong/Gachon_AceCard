using UnityEngine;

public class TransitionManagerInitializer : MonoBehaviour
{
    public GameObject transitionManagerPrefab; // TransitionManager 프리팹

    private void Awake()
    {
        // TransitionManager 인스턴스가 없을 경우 생성
        if (TransitionManager.Instance == null)
        {
            GameObject transitionManager = Instantiate(transitionManagerPrefab);
            transitionManager.SetActive(true); // TransitionManager를 활성화
            Debug.Log("TransitionManagerInitializer: TransitionManager prefab instantiated.");
        }
    }
}
    