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

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupWelcomePacket();
            SetupAttackEventHandler();
            SetupSkillEventHandler();
            SetupSwitchEventHandler();
            SetupTurnEventHandler();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 연결 함수
    public void Connect(string ipAddress, int port)
    {
        _client = new TcpClient
        {
            ReceiveBufferSize = 4096,
            SendBufferSize = 4096,
        };
        _client.BeginConnect(ipAddress, port, ConnectionCallback, null);

        DontDestroyOnLoad(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("CardTestScene");
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

    // 패킷 콜백 함수
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
    
    // 플레이어 ID 반환 함수
    public int GetPlayerID() {
        return _playerID; // 플레이어 ID 반환
    }
    
    
    // 이벤트 함수들
    public void SendAttackEvent(int attackerPid, int damage)
    {
        Utilities.AttackEvent attackEvent = new Utilities.AttackEvent
        {
            attackPlayerID = attackerPid,
            damage = damage
        };

        string json = JsonUtility.ToJson(attackEvent);
        Packet packet = new Packet();
        packet.Write((int)PacketType.Attack); // 패킷 유형
        packet.Write(json); // JSON 데이터
        Debug.Log($"클라이언트에서 서버로 공격 이벤트 전송: {json}");

        // 서버로 데이터 전송
        _stream.Write(packet.ToArray(), 0, packet.ToArray().Length);
    }
    
    public void SendSkillEvent(int attackerPid, string type)
    {
        Utilities.SkillEvent attackEvent = new Utilities.SkillEvent
        {
            attackPlayerID = attackerPid,
            type = type
        };

        string json = JsonUtility.ToJson(attackEvent);
        Packet packet = new Packet();
        packet.Write((int)PacketType.Skill); // 패킷 유형
        packet.Write(json); // JSON 데이터
        Debug.Log($"클라이언트에서 서버로 공격 이벤트 전송: {json}");

        // 서버로 데이터 전송
        _stream.Write(packet.ToArray(), 0, packet.ToArray().Length);
    }
    
    public void SendSwitchEvent(int playerId, int cardId)
    {
        Utilities.SwitchEvent switchEvent = new Utilities.SwitchEvent
        {
            playerID = playerId,
            switchIndexed = cardId
        };

        string json = JsonUtility.ToJson(switchEvent);
        Packet packet = new Packet();
        packet.Write((int)PacketType.Switch); // 패킷 유형
        packet.Write(json); // JSON 데이터
        Debug.Log($"클라이언트에서 서버로 교체 이벤트 전송: {json}");

        // 서버로 데이터 전송
        _stream.Write(packet.ToArray(), 0, packet.ToArray().Length);
    }

    public void SendTurnEndRequest(int playerId)
    {
        int newTurn = playerId == 1 ? 2 : 1;
        
        Utilities.TurnEvent turnEvent = new Utilities.TurnEvent
        {
            currentTurnPlayerID = newTurn
        };

        string json = JsonUtility.ToJson(turnEvent);
        Packet packet = new Packet();
        packet.Write((int)PacketType.Turn); // 패킷 유형
        packet.Write(json); // JSON 데이터
        
        // 서버로 데이터 전송
        _stream.Write(packet.ToArray(), 0, packet.ToArray().Length);
    }


    // 패킷 등록
    private void SetupWelcomePacket()
    {
        RegisterAction((int)PacketType.Welcome, data =>
        {
            Packet packet = new Packet(data);
            var pacType = packet.ReadInt();
            _playerID = packet.ReadInt(); // 서버로부터 PlayerID 읽기
            Debug.Log($"서버로부터 받은 Player ID: {_playerID}");
        });
    }
    
    private void SetupTurnEventHandler()
    {
        RegisterAction((int)PacketType.Turn, data =>
        {
            Packet packet = new Packet(data);
            int type = packet.ReadInt();
            string json = packet.ReadString();
            Utilities.TurnEvent turnEvent = JsonUtility.FromJson<Utilities.TurnEvent>(json);

            Debug.Log($"[Client] Received Turn Info: Player {turnEvent.currentTurnPlayerID}");

            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                GameManager.Instance.UpdateTurnUI(turnEvent.currentTurnPlayerID);
            });
        });
    }
    
    private void SetupAttackEventHandler()
    {
        RegisterAction((int)PacketType.Attack, data =>
        {
            Packet packet = new Packet(data);
            int type = packet.ReadInt();
            string json = packet.ReadString(); 
            Utilities.AttackEvent attackEvent = JsonUtility.FromJson<Utilities.AttackEvent>(json);

            Debug.Log($"받은 Attack Event: Player {attackEvent.attackPlayerID}, Damage: {attackEvent.damage}");

            // 타겟 카드에 데미지 반영
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                CharacterCard targetCard = FindCardById(attackEvent.attackPlayerID);
                if (targetCard != null)
                {
                    targetCard.TakeDamage(attackEvent.damage);
                }
            });
        });
    }
    
    private void SetupSkillEventHandler()
    {
        RegisterAction((int)PacketType.Skill, data =>
        {
            Packet packet = new Packet(data);
            int type = packet.ReadInt();
            string json = packet.ReadString();
            Utilities.SkillEvent attackEvent = JsonUtility.FromJson<Utilities.SkillEvent>(json);

            Debug.Log($"받은 Attack Event: Player {attackEvent.attackPlayerID}");

            // 타겟 카드에 데미지 반영
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                CharacterCard myCard = FindMyCardById(attackEvent.attackPlayerID);
                CharacterCard targetCard = FindCardById(attackEvent.attackPlayerID);
                if (targetCard != null)
                {
                    myCard.skillCard.SelectTarget(targetCard);
                }
            });
        });
    }
    
    private void SetupSwitchEventHandler()
    {
        RegisterAction((int)PacketType.Switch, data =>
        {
            Packet packet = new Packet(data);
            int type = packet.ReadInt();
            string json = packet.ReadString();
            Utilities.SwitchEvent switchEvent = JsonUtility.FromJson<Utilities.SwitchEvent>(json);

            Debug.Log($"받은 Switch Event: Player {switchEvent.playerID}, Card {switchEvent.switchIndexed}");

            // 타겟 카드 교체 반영
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                GameManager.Instance.ApplySwitch(switchEvent);
            });
        });
    }

    // 카드 찾는 함수    
    private CharacterCard FindMyCardById(int pid)
    {
        int playerId = pid == 1 ? 1 : 2; // 상대 플레이어의 ID 계산
        Debug.Log($"타겟 찾기: Player{playerId}");
        
        GameObject myParticipation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Participation").gameObject;
        
        var myCards = myParticipation.GetComponentsInChildren<CharacterCard>();
        if (myCards.Length > 0)
        {
            return myCards[0]; // 첫 번째 카드를 반환
        }
        
        Debug.LogError("Participation에 카드가 없습니다.");
        return null;
    }
    
    private CharacterCard FindCardById(int pid)
    {
        int opponentId = pid == 1 ? 2 : 1; // 상대 플레이어의 ID 계산
        Debug.Log($"타겟 찾기: Player{opponentId}");

        GameObject opponentParticipation = GameObject.FindGameObjectsWithTag("Player" + opponentId)[0]
            .transform.Find("Participation").gameObject;

        var opponentCards = opponentParticipation.GetComponentsInChildren<CharacterCard>();
        if (opponentCards.Length > 0)
        {
            return opponentCards[0]; // 첫 번째 카드를 반환
        }

        Debug.LogError("상대 Participation에 카드가 없습니다.");
        return null;
    }
    
    // 기타 유틸리티
    private void Update()
    {
        if (_shouldLoadScene)
        {
            _shouldLoadScene = false; // 플래그 초기화
            // SceneManager.LoadScene("GameScene"); // 실제 GameScene 이름으로 변경
        }
    }
    
    private void OnApplicationQuit()
    {
        _stream?.Close();
        _client?.Close();
    }
    
    public void RegisterAction(int packetType, Action<byte[]> action)
    {
        _connectedActions.TryAdd(packetType, action);
    }
}
