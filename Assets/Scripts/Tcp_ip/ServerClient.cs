using System;
using System.Net.Sockets;
using UnityEngine;

public class ServerClient
{
    private static int _idCounter = 1; // ���� ID ������ ī����
    public int PlayerID { get; private set; } // �� Ŭ���̾�Ʈ�� ���� ID

    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private byte[] _receiveBuffer;

    public ServerClient(TcpClient client)
    {
        _tcpClient = client;
        _stream = _tcpClient.GetStream();
        _receiveBuffer = new byte[4096];

        PlayerID = _idCounter++; // ���� ID �Ҵ�
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
        _stream.BeginWrite(data, 0, data.Length, OnWriteComplete, null);
    }
    
    private void OnWriteComplete(IAsyncResult ar)
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

            Packet packet = new Packet(data);
            int commandID = packet.ReadInt(); // ��Ŷ�� ù ��° ��: PacketType
            if (commandID == (int)PacketType.Attack)
            {
                Server.Instance.HandleAttackEvent(data); // HandleAttackEvent ȣ��
            }
            if (commandID == (int)PacketType.Skill)
            {
                Server.Instance.HandleSkillEvent(data); // HandleSkillEvent ȣ��
            }
            if (commandID == (int)PacketType.Switch)
            {
                Server.Instance.HandleSwitchEvent(data); // HandleSwitchEvent ȣ��
            }
            if (commandID == (int)PacketType.Turn)
            {
                Server.Instance.HandleTurnEvent(data); // EndTurn ��û ó��
            }
            if (commandID == (int)PacketType.CostUpdate)
            {
                Server.Instance.HandleCostEvent(data);
            }
            if (commandID == (int)PacketType.CostAdd)
            {
                Server.Instance.HandleCostAddEvent(data);
            }
            if (commandID == (int)PacketType.AddHp)
            {
                Server.Instance.HandleHpAddEvent(data);
            }
            StartRead(); // ���� ������ ���� ����
        }
        catch (Exception e)
        {
            Debug.LogError($"Client {PlayerID} Receive Error: {e.Message}");
        }
    }
}
