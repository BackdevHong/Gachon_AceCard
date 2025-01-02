using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClientManager : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    public string serverIP = "127.0.0.1";
    public int port = 8080;
    public static TCPClientManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ConnectToServer();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverIP, port);
            stream = client.GetStream();
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
            Debug.Log($"서버 {serverIP}:{port}에 연결되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"서버 연결 중 오류: {ex.Message}");
        }
    }

    private void ReceiveData()
    {
        try
        {
            while (client != null && client.Connected)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log($"서버로부터 메시지 수신: {message}");
                    ProcessReceivedMessage(message);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"데이터 수신 중 오류: {ex.Message}");
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogWarning("서버에 연결되지 않았습니다.");
            return;
        }

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log($"서버로 메시지 전송: {message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"메시지 전송 중 오류: {ex.Message}");
        }
    }

    private void ProcessReceivedMessage(string message)
    {
        GameEvent gameEvent = JsonUtility.FromJson<GameEvent>(message);
        GameManager.Instance.HandleGameEvent(gameEvent);
    }

    public void Disconnect()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }

        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (client != null)
        {
            client.Close();
            client = null;
        }

        Debug.Log("서버와의 연결이 종료되었습니다.");
    }
}

[Serializable]
public class GameEvent
{
    public string eventType; // "Attack", "Skill", "Switch"
    public CharactorCard attacker;
    public CharactorCard target;
    public int damage; // 공격 또는 스킬 데미지
    public string switchFrom;
    public string switchTo;
}
