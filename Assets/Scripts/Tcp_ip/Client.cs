using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static Client Instance { get; private set; } // 싱글톤

    private TcpClient _client;
    private NetworkStream _stream;
    private byte[] _receiveBuffer;
    private bool _connected;

    private Dictionary<int, Action<byte[]>> _connectedActions = new Dictionary<int, Action<byte[]>>();

    private bool _shouldLoadScene = false; // 씬 로딩 플래그
    private int _playerID; // 서버에서 받은 고유 ID

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Connect(string ipAddress, int port)
    {
        _client = new TcpClient
        {
            ReceiveBufferSize = 4096,
            SendBufferSize = 4096,
        };
        _client.BeginConnect(ipAddress, port, ConnectionCallback, null);
    }

    private void ConnectionCallback(IAsyncResult ar)
    {
        try
        {
            _client.EndConnect(ar);
            _stream = _client.GetStream();
            _connected = true;
            Debug.Log($"Connected to Server: {_client.Client.RemoteEndPoint}");

            _shouldLoadScene = true; // 씬 로딩 플래그 설정
            StartRead();
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection Error: {e.Message}");
        }
    }

    private void StartRead()
    {
        _receiveBuffer = new byte[4096];
        _stream.BeginRead(_receiveBuffer, 0, _receiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int packetLength = _stream.EndRead(ar);
            if (packetLength <= 0)
            {
                Debug.Log("Disconnected from server.");
                _connected = false;
                return;
            }

            byte[] data = new byte[packetLength];
            Array.Copy(_receiveBuffer, data, packetLength);

            Packet packet = new Packet(data);
            int commandID = packet.ReadInt();

            if (_connectedActions.ContainsKey(commandID))
            {
                _connectedActions[commandID]?.Invoke(data);
            }

            StartRead();
        }
        catch (Exception e)
        {
            Debug.LogError($"Receive Error: {e.Message}");
        }
    }


    private void Update()
    {
        if (_shouldLoadScene)
        {
            _shouldLoadScene = false; // 플래그 초기화
            SceneManager.LoadScene("GameScene"); // 실제 GameScene 이름으로 변경
        }
    }

    public int GetPlayerID()
    {
        return _playerID; // 플레이어 ID 반환
    }

    private void OnApplicationQuit()
    {
        _stream?.Close();
        _client?.Close();
    }
    public void RegisterAction(int packetType, Action<byte[]> action)
    {
        if (!_connectedActions.ContainsKey(packetType))
        {
            _connectedActions.Add(packetType, action);
        }
    }
}
