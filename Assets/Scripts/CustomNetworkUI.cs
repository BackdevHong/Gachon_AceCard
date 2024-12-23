using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkUI : MonoBehaviour
{
    public NetworkManager manager;
    public TMP_InputField ipInputField;
    public TMP_Text statusText;
    
    private string publicIPMessage = "";

    public void StartHost()
    {
        if (!manager.isNetworkActive)
        {
            manager.StartHost();
            StartCoroutine(ShowPublicIP()); // 공인 IP 가져오기
        }
    }
    
    private IEnumerator ShowPublicIP()
    {
        string url = "https://api64.ipify.org"; // 공인 IP를 반환하는 API

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                publicIPMessage = $"Host started. Public IP: {request.downloadHandler.text}";
                UpdateStatus();
            }
            else
            {
                publicIPMessage = "Failed to retrieve public IP.";
                UpdateStatus();
            }
        }
    }

    public void StartClient()
    {
        if (!manager.isNetworkActive)
        {
            string ipAddress = ipInputField.text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
            {
                AddMessage("Please enter a valid IP Address.");
            }
            
            manager.networkAddress = ipAddress; // NetworkManager에 IP 주소 설정
            manager.StartClient(); // 클라이언트 시작
            AddMessage($"Connecting to server at {ipAddress}...");
        }
    }

    // 네트워크 종료
    public void StopNetworking()
    {
        if (manager.isNetworkActive)
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                manager.StopHost();
                AddMessage("Host stopped.");
            }
            else if (NetworkClient.isConnected)
            {
                manager.StopClient();
                AddMessage("Client disconnected.");
            }
            else if (NetworkServer.active)
            {
                manager.StopServer();
                AddMessage("Server stopped.");
            }
        }
    }

    // 서버에 플레이어 추가될 때 호출
    public void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // 접속 정보 업데이트
        AddMessage($"Player connected: {conn.address}");
    }
    
    private void AddMessage(string message)
    {
        statusText.text += message + "\n";
        Debug.Log(message); // 콘솔에도 출력
    }

    // 상태 업데이트 (공인 IP는 첫 번째 줄)
    private void UpdateStatus()
    {
        statusText.text = publicIPMessage + "\n"; // 첫 줄: 공인 IP 메시지
        Debug.Log(publicIPMessage); // 콘솔에도 출력
    }
}
