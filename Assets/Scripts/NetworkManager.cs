using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManager : Mirror.NetworkManager
{
    public void OnConnectedToServer()
    {
        Debug.Log("서버 연결에 성공함");
        throw new NotImplementedException();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("서버 시작");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("서버 종료");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("클라이언트 서버 연결");
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("클라이언트 서버 연결 해제");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log("플레이어가 서버에 연결하였습니다.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
