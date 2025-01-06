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
    
    // ���� �Լ�
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

            _shouldLoadScene = true; // �� �ε� �÷��� ����
            StartRead();
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection Error: {e.Message}");
        }
    }

    // ��Ŷ �ݹ� �Լ�
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
    
    // �÷��̾� ID ��ȯ �Լ�
    public int GetPlayerID() {
        return _playerID; // �÷��̾� ID ��ȯ
    }
    
    
    // �̺�Ʈ �Լ���
    public void SendAttackEvent(int attackerPid, int damage)
    {
        Utilities.AttackEvent attackEvent = new Utilities.AttackEvent
        {
            attackPlayerID = attackerPid,
            damage = damage
        };

        string json = JsonUtility.ToJson(attackEvent);
        Packet packet = new Packet();
        packet.Write((int)PacketType.Attack); // ��Ŷ ����
        packet.Write(json); // JSON ������
        Debug.Log($"Ŭ���̾�Ʈ���� ������ ���� �̺�Ʈ ����: {json}");

        // ������ ������ ����
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
        packet.Write((int)PacketType.Skill); // ��Ŷ ����
        packet.Write(json); // JSON ������
        Debug.Log($"Ŭ���̾�Ʈ���� ������ ���� �̺�Ʈ ����: {json}");

        // ������ ������ ����
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
        packet.Write((int)PacketType.Switch); // ��Ŷ ����
        packet.Write(json); // JSON ������
        Debug.Log($"Ŭ���̾�Ʈ���� ������ ��ü �̺�Ʈ ����: {json}");

        // ������ ������ ����
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
        packet.Write((int)PacketType.Turn); // ��Ŷ ����
        packet.Write(json); // JSON ������
        
        // ������ ������ ����
        _stream.Write(packet.ToArray(), 0, packet.ToArray().Length);
    }


    // ��Ŷ ���
    private void SetupWelcomePacket()
    {
        RegisterAction((int)PacketType.Welcome, data =>
        {
            Packet packet = new Packet(data);
            var pacType = packet.ReadInt();
            _playerID = packet.ReadInt(); // �����κ��� PlayerID �б�
            Debug.Log($"�����κ��� ���� Player ID: {_playerID}");
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

            Debug.Log($"���� Attack Event: Player {attackEvent.attackPlayerID}, Damage: {attackEvent.damage}");

            // Ÿ�� ī�忡 ������ �ݿ�
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

            Debug.Log($"���� Attack Event: Player {attackEvent.attackPlayerID}");

            // Ÿ�� ī�忡 ������ �ݿ�
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

            Debug.Log($"���� Switch Event: Player {switchEvent.playerID}, Card {switchEvent.switchIndexed}");

            // Ÿ�� ī�� ��ü �ݿ�
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                GameManager.Instance.ApplySwitch(switchEvent);
            });
        });
    }

    // ī�� ã�� �Լ�    
    private CharacterCard FindMyCardById(int pid)
    {
        int playerId = pid == 1 ? 1 : 2; // ��� �÷��̾��� ID ���
        Debug.Log($"Ÿ�� ã��: Player{playerId}");
        
        GameObject myParticipation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Participation").gameObject;
        
        var myCards = myParticipation.GetComponentsInChildren<CharacterCard>();
        if (myCards.Length > 0)
        {
            return myCards[0]; // ù ��° ī�带 ��ȯ
        }
        
        Debug.LogError("Participation�� ī�尡 �����ϴ�.");
        return null;
    }
    
    private CharacterCard FindCardById(int pid)
    {
        int opponentId = pid == 1 ? 2 : 1; // ��� �÷��̾��� ID ���
        Debug.Log($"Ÿ�� ã��: Player{opponentId}");

        GameObject opponentParticipation = GameObject.FindGameObjectsWithTag("Player" + opponentId)[0]
            .transform.Find("Participation").gameObject;

        var opponentCards = opponentParticipation.GetComponentsInChildren<CharacterCard>();
        if (opponentCards.Length > 0)
        {
            return opponentCards[0]; // ù ��° ī�带 ��ȯ
        }

        Debug.LogError("��� Participation�� ī�尡 �����ϴ�.");
        return null;
    }
    
    // ��Ÿ ��ƿ��Ƽ
    private void Update()
    {
        if (_shouldLoadScene)
        {
            _shouldLoadScene = false; // �÷��� �ʱ�ȭ
            // SceneManager.LoadScene("GameScene"); // ���� GameScene �̸����� ����
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
