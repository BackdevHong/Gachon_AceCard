using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    // 현재 연결된 클라이언트 목록
    private List<NetworkConnectionToClient> connectedClients = new List<NetworkConnectionToClient>();

    // 서버 시작 시 호출
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("서버가 시작되었습니다.");
    }

    // 서버 종료 시 호출
    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("서버가 종료되었습니다.");
        connectedClients.Clear(); // 연결된 클라이언트 목록 초기화
    }

    // 클라이언트가 서버에 연결 성공 시 호출
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("클라이언트가 서버에 연결되었습니다.");
    }

    // 클라이언트가 서버와 연결 해제 시 호출
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("클라이언트가 서버에서 연결 해제되었습니다.");
    }

    // 서버에 클라이언트가 연결될 때 호출
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // 로그 출력
        Debug.Log($"플레이어가 서버에 연결되었습니다. IP: {conn.address}");

        // 연결된 클라이언트 목록에 추가
        connectedClients.Add(conn);
        
        // 현재 연결된 클라이언트 수 출력
        Debug.Log($"현재 연결된 클라이언트 수: {connectedClients.Count}");
    }

    // 서버에서 클라이언트가 연결 해제될 때 호출
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        // 클라이언트 목록에서 제거
        if (connectedClients.Contains(conn))
        {
            connectedClients.Remove(conn);
            Debug.Log($"플레이어가 서버에서 연결 해제되었습니다. IP: {conn.address}");
        }

        // 현재 연결된 클라이언트 수 출력
        Debug.Log($"현재 연결된 클라이언트 수: {connectedClients.Count}");
    }
}