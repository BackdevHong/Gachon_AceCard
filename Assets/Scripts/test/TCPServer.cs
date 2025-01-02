using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPServer : MonoBehaviour
{
    private TcpListener server;
    private Thread serverThread;
    public int port = 8080;

    private void Start()
    {
        StartServer();
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }

    public void StartServer()
    {
        serverThread = new Thread(() =>
        {
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Debug.Log($"서버가 {port} 포트에서 실행 중입니다.");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Debug.Log("클라이언트가 연결되었습니다.");
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"서버 실행 중 오류: {ex.Message}");
            }
        });

        serverThread.IsBackground = true;
        serverThread.Start();
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (client.Connected)
        {
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log($"클라이언트로부터 수신된 메시지: {message}");

                // Echo: 클라이언트에게 다시 전송
                byte[] response = Encoding.UTF8.GetBytes($"서버 응답: {message}");
                stream.Write(response, 0, response.Length);
            }
            catch (Exception ex)
            {
                Debug.LogError($"클라이언트 처리 중 오류: {ex.Message}");
                break;
            }
        }

        Debug.Log("클라이언트 연결이 종료되었습니다.");
        client.Close();
    }

    public void StopServer()
    {
        if (server != null)
        {
            server.Stop();
            server = null;
        }

        if (serverThread != null)
        {
            serverThread.Abort();
            serverThread = null;
        }

        Debug.Log("서버가 중지되었습니다.");
    }
}