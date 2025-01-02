using System;
using System.Net.Sockets;
using UnityEngine;

public class ServerClient
{
    private static int _idCounter = 1; // 고유 ID 생성용 카운터
    public int PlayerID { get; private set; } // 각 클라이언트의 고유 ID

    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private byte[] _receiveBuffer;

    public ServerClient(TcpClient client)
    {
        _tcpClient = client;
        _stream = _tcpClient.GetStream();
        _receiveBuffer = new byte[4096];

        PlayerID = _idCounter++; // 고유 ID 할당
        Debug.Log($"Assigned Player ID: {PlayerID}");

        StartRead();
    }

    private void StartRead()
    {
        _stream.BeginRead(_receiveBuffer, 0, _receiveBuffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int packetLength = _stream.EndRead(ar);
            if (packetLength <= 0)
            {
                Debug.Log($"Player {PlayerID} disconnected.");
                return;
            }

            byte[] data = new byte[packetLength];
            Array.Copy(_receiveBuffer, data, packetLength);

            Server.Instance.Broadcast(data);
            StartRead();
        }
        catch (Exception e)
        {
            Debug.LogError($"Client {PlayerID} Receive Error: {e.Message}");
        }
    }

    public void SendData(byte[] data)
    {
        _stream.BeginWrite(data, 0, data.Length, null, null);
    }
}
