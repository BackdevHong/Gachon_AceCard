using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    GameManager _gameManager = GameManager.Instance;
    
    public TMP_Text player1CostText;
    public TMP_Text player2CostText;

    public GameObject player1Preparation;
    public GameObject player2Preparation;

    public Button endTurnButton;
    
    public static GameUIManager Instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager.player1cost = player1CostText;
        _gameManager.player2cost = player2CostText;

        _gameManager.player1Preparation = player1Preparation;
        _gameManager.player2Preparation = player2Preparation;
        
        _gameManager.turnEndButton = endTurnButton;
        
        endTurnButton.onClick.AddListener(() =>
        {
            Client.Instance.SendTurnEndRequest();
        });
        _gameManager.UpdateCostUI();
        _gameManager.UpdateTurnUI();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        SetupPlayerPositions();
    }
    
    public void SetupPlayerPositions()
    {
        int playerId = Client.Instance.GetPlayerID(); // ���� �÷��̾� ID
        int opponentId = playerId == 1 ? 2 : 1; // ���� ID
        
        if (Client.Instance.ready) return;
        
        if (opponentId == 2)
        {
            // �ڽ��� UI�� �׻� �Ʒ���, ���� UI�� ���� ��ġ
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            // ��ġ ����
            player.transform.position = new Vector3(-2.1319f, 0f, 0); // �ڽ��� ī�� (�Ʒ���)
            opponent.transform.position = new Vector3(-2.1319f, 0f, 0); // ������ ī�� (����)
        }
        else
        {
            // �ڽ��� UI�� �׻� �Ʒ���, ���� UI�� ���� ��ġ
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            player.transform.Find("Preparation").transform.position = new Vector3(-2.1319f, -3.4f, 0); // �ڽ��� Preparation (���� )
            opponent.transform.Find("Preparation").transform.position = new Vector3(-2.1319f, 3.4f, 0); // �ڽ��� Participation (�Ʒ��� )
            
            player.transform.Find("Participation").transform.position = new Vector3(-2.1319f, -2f, 0); // �ڽ��� Preparation (�Ʒ���)
            opponent.transform.Find("Participation").transform.position = new Vector3(-2.1319f, 1.4f, 0); // �ڽ��� Preparation (�Ʒ���)
            // ��ġ ����
            // player.transform.position = new Vector3(-1.92f, -4f, 0); // �ڽ��� ī�� (�Ʒ���)
            // opponent.transform.position = new Vector3(-1.92f, 4f, 0); // ������ ī�� (����)
        }

        Debug.Log($"Player {playerId} ��ġ: �Ʒ���, Player {opponentId} ��ġ: ����");
    }
    
    public List<CharacterCard> GetAllCards(int playerID)
    {
        List<CharacterCard> allCards = new List<CharacterCard>();

        // �÷��̾��� Preparation�� Participation ������Ʈ ��������
        GameObject playerObject = GameObject.FindGameObjectsWithTag("Player" + playerID)[0];
        GameObject preparation = playerObject.transform.Find("Preparation").gameObject;
        GameObject participation = playerObject.transform.Find("Participation").gameObject;

        // Preparation���� ��� CharacterCard ����
        var preparationCards = preparation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(preparationCards);

        // Participation���� ��� CharacterCard ����
        var participationCards = participation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(participationCards);

        return allCards;
    }
}
