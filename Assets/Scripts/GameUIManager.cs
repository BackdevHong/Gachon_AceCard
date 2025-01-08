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
        int playerId = Client.Instance.GetPlayerID(); // 현재 플레이어 ID
        int opponentId = playerId == 1 ? 2 : 1; // 상대방 ID
        
        if (Client.Instance.ready) return;
        
        if (opponentId == 2)
        {
            // 자신의 UI를 항상 아래로, 상대방 UI를 위로 배치
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            // 위치 설정
            player.transform.position = new Vector3(-2.1319f, 0f, 0); // 자신의 카드 (아래쪽)
            opponent.transform.position = new Vector3(-2.1319f, 0f, 0); // 상대방의 카드 (위쪽)
        }
        else
        {
            // 자신의 UI를 항상 아래로, 상대방 UI를 위로 배치
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            player.transform.Find("Preparation").transform.position = new Vector3(-2.1319f, -3.4f, 0); // 자신의 Preparation (위쪽 )
            opponent.transform.Find("Preparation").transform.position = new Vector3(-2.1319f, 3.4f, 0); // 자신의 Participation (아래쪽 )
            
            player.transform.Find("Participation").transform.position = new Vector3(-2.1319f, -2f, 0); // 자신의 Preparation (아래쪽)
            opponent.transform.Find("Participation").transform.position = new Vector3(-2.1319f, 1.4f, 0); // 자신의 Preparation (아래쪽)
            // 위치 설정
            // player.transform.position = new Vector3(-1.92f, -4f, 0); // 자신의 카드 (아래쪽)
            // opponent.transform.position = new Vector3(-1.92f, 4f, 0); // 상대방의 카드 (위쪽)
        }

        Debug.Log($"Player {playerId} 위치: 아래쪽, Player {opponentId} 위치: 위쪽");
    }
    
    public List<CharacterCard> GetAllCards(int playerID)
    {
        List<CharacterCard> allCards = new List<CharacterCard>();

        // 플레이어의 Preparation과 Participation 오브젝트 가져오기
        GameObject playerObject = GameObject.FindGameObjectsWithTag("Player" + playerID)[0];
        GameObject preparation = playerObject.transform.Find("Preparation").gameObject;
        GameObject participation = playerObject.transform.Find("Participation").gameObject;

        // Preparation에서 모든 CharacterCard 수집
        var preparationCards = preparation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(preparationCards);

        // Participation에서 모든 CharacterCard 수집
        var participationCards = participation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(participationCards);

        return allCards;
    }
}
