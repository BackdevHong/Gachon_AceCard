using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    public string sceneToLoad;

    public void OnButtonClick()
    {
        Debug.Log($"SceneChangeButton: Button clicked. Target scene = '{sceneToLoad}'.");

        if (TransitionManager.Instance != null)
        {
            Debug.Log("SceneChangeButton: TransitionManager instance found. Calling LoadSceneWithFade.");
            TransitionManager.Instance.LoadSceneWithFade(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneChangeButton: TransitionManager instance is not available!");
        }
    }
}
