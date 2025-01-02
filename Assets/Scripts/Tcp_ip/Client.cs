using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static Client Instance { get; private set; } // �̱���

    private TcpClient _client;
    private NetworkStream _stream;
    private byte[] _receiveBuffer;
    private bool _connected;

    private Dictionary<int, Action<byte[]>> _connectedActions = new Dictionary<int, Action<byte[]>>();

    private bool _shouldLoadScene = false; // �� �ε� �÷���
    private int _playerID; // �������� ���� ���� ID

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

            _shouldLoadScene = true; // �� �ε� �÷��� ����
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
            _shouldLoadScene = false; // �÷��� �ʱ�ȭ
            SceneManager.LoadScene("GameScene"); // ���� GameScene �̸����� ����
        }
    }

    public int GetPlayerID()
    {
        return _playerID; // �÷��̾� ID ��ȯ
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
