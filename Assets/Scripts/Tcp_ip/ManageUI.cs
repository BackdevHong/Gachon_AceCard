using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManageUI : MonoBehaviour
{
    public TMP_InputField ipField;
    public TMP_InputField portField;
    public Button serverButton;
    public Button clientButton;

    private void Start()
    {
        serverButton.onClick.AddListener(() =>
        {
            if (ObjectSelector.Instance.GetSelectedObjects() == null || ObjectSelector.Instance.GetSelectedObjects().Count == 0)
            {
                Debug.Log("카드를 먼저 선택해주세요!");
                return;
            }
            Server.Instance.CreateServer();
        });

        clientButton.onClick.AddListener(() =>
        {
            if (ObjectSelector.Instance.GetSelectedObjects() == null || ObjectSelector.Instance.GetSelectedObjects().Count == 0)
            {
                Debug.Log("카드를 먼저 선택해주세요!");
                return;
            }
            string ip = ipField.text == "" ? "127.0.0.1" : ipField.text;
            int port = portField.text == "" ? 7777 : int.Parse(portField.text);
            Client.Instance.Connect(ip, port);
        });
    }
}
