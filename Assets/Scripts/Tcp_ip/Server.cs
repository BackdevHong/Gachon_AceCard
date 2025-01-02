using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    public static Server Instance { get; private set; }

    private TcpListener _server;
    private List<ServerClient> _clients = new List<ServerClient>();
    private const int Port = 7777;

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

    public void CreateServer()
    {
        _server = new TcpListener(IPAddress.Any, Port);
        _server.Start();
        Debug.Log("Server Started.");
        StartListening();
    }

    private void StartListening()
    {
        _server.BeginAcceptTcpClient(AcceptClientCallback, null);
    }

    private void AcceptClientCallback(IAsyncResult ar)
    {
        TcpClient newClient = _server.EndAcceptTcpClient(ar);
        ServerClient serverClient = new ServerClient(newClient);

        // 고유 ID를 클라이언트에 전송
        Packet packet = new Packet();
        packet.Write((int)PacketType.Welcome);
        packet.Write(serverClient.PlayerID); // 클라이언트에 ID 전송
        serverClient.SendData(packet.ToArray());

        _clients.Add(serverClient);
        Debug.Log($"Client Connected: {newClient.Client.RemoteEndPoint} with Player ID: {serverClient.PlayerID}");

        StartListening();
    }

    public void Broadcast(byte[] data)
    {
        foreach (ServerClient client in _clients)
        {
            client.SendData(data);
        }
    }

    private void OnApplicationQuit()
    {
        _server?.Stop();
    }
}
