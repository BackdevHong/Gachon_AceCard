using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player1Preparation; // Player1의 Preparation 오브젝트
    public GameObject player2Preparation; // Player2의 Preparation 오브젝트

    public TMP_Text player1cost;
    public TMP_Text player2cost;
    public Button turnEndButton;

    private CharacterCard selectedPreparationCard; // 선택된 Preparation 카드
    public bool isSwitching = false; // 교체 모드 활성화 여부
    public static GameManager Instance;

    private int _turnC = -1;
    private int _Round = 0;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateTurnUI();
            Client.Instance.SendReadyPacket();

        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        turnEndButton.onClick.AddListener(() =>
        {
            Client.Instance.SendTurnEndRequest();
        });
        UpdateCostUI();
    }
    
    public void SetupPlayerPositions()
    {
        int playerId = Client.Instance.GetPlayerID(); // 현재 플레이어 ID
        int opponentId = playerId == 1 ? 2 : 1; // 상대방 ID

        if (opponentId == 2)
        {
            // 자신의 UI를 항상 아래로, 상대방 UI를 위로 배치
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            // 위치 설정
            player.transform.position = new Vector3(-1.92f, 0f, 0); // 자신의 카드 (아래쪽)
            opponent.transform.position = new Vector3(-1.92f, 0f, 0); // 상대방의 카드 (위쪽)
        }
        else
        {
            // 자신의 UI를 항상 아래로, 상대방 UI를 위로 배치
            GameObject player = GameObject.FindWithTag($"Player{playerId}");
            GameObject opponent = GameObject.FindWithTag($"Player{opponentId}");

            // 위치 설정
            player.transform.position = new Vector3(-1.92f, -6.2f, 0); // 자신의 카드 (아래쪽)
            opponent.transform.position = new Vector3(-1.92f, 6.2f, 0); // 상대방의 카드 (위쪽)
        }
        
        Debug.Log($"Player {playerId} 위치: 아래쪽, Player {opponentId} 위치: 위쪽");
    }
    
    public void AdjustCardRotation()
    {
        int playerId = Client.Instance.GetPlayerID();
        int opponentId = playerId == 1 ? 2 : 1;
        
        foreach (var characterCard in GetAllCards(opponentId))
        {
            characterCard.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    public void Connect()
    {
        Client.Instance.Connect("localhost", 7777);
    }

    public void StartServer()
    {
        Server.Instance.CreateServer();
    }

    public void StartSwitch()
    {
        if(Client.Instance.GetCurrentTurnPlayerID() != Client.Instance.GetPlayerID()) return;
        isSwitching = true; // 교체 모드 활성화
    }

    public void SelectPreparationCard(CharacterCard card)
    {
        if (!isSwitching)
        {
            Debug.Log("교체 모드가 아닙니다.");
            return;
        }

        // 현재 턴에 따라 올바른 Preparation 영역 확인
        int playerId = Client.Instance.GetPlayerID();
        GameObject currentPreparation = playerId == 1 ? player1Preparation : player2Preparation;

        if (card.transform.parent != currentPreparation.transform)
        {
            Debug.Log("현재 턴의 Preparation에 있는 카드만 선택할 수 있습니다.");
            return;
        }

        selectedPreparationCard = card;
        SwitchCard(selectedPreparationCard.transform.GetSiblingIndex());
    }
    
    public void SwitchCard(int cardId)
    {
        int playerId = Client.Instance.GetPlayerID();
        Debug.Log($"Player {playerId} 카드 교체 요청: {cardId}");
        Client.Instance.SendSwitchEvent(playerId, cardId);
    }

    public void ApplySwitch(Utilities.SwitchEvent switchEvent)
    {
        int playerId = switchEvent.playerID;
        GameObject playerParticipation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Participation").gameObject;
        GameObject playerPreparation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Preparation").gameObject;

        var participationCards = playerParticipation.GetComponentsInChildren<CharacterCard>();
        var preparationCards = playerPreparation.GetComponentsInChildren<CharacterCard>();

        if (participationCards.Length > 0 && preparationCards.Length > 0)
        {
            var targetCard = Array.Find(preparationCards, c => c.transform.GetSiblingIndex() == switchEvent.switchIndexed);
            if (targetCard == null) return;

            // 위치 교환
            Vector3 participationPosition = participationCards[0].transform.position;
            Vector3 preparationPosition = targetCard.transform.position;

            Transform participationParent = participationCards[0].transform.parent;
            Transform preparationParent = targetCard.transform.parent;

            targetCard.transform.SetParent(participationParent);
            participationCards[0].transform.SetParent(preparationParent);

            targetCard.transform.position = participationPosition;
            participationCards[0].transform.position = preparationPosition;

            Debug.Log($"Player {playerId} 카드 교체 완료: {participationCards[0].name} <-> {targetCard.name}");
            Client.Instance.SendUpdateCostEvent(1);
        }
    }

    public void NormalAttack()
    {   
        Debug.Log($"Current Turn: {Client.Instance.GetCurrentTurnPlayerID()}, Local Player: {Client.Instance.GetPlayerID()}");
        if (Client.Instance.GetCurrentTurnPlayerID() != Client.Instance.GetPlayerID())
        {
            Debug.Log("내 턴이 아닙니다. 스킬을 사용할 수 없습니다.");
            return;
        }
        int playerId = Client.Instance.GetPlayerID();
        if (!Client.Instance.PlayerCosts.TryGetValue(playerId, out int playerCost))
        {
            Debug.LogError($"Player {playerId} not found in _playerCosts.");
            return;
        }

        Debug.Log($"Player {playerId} Current Cost: {playerCost}");

        if (playerCost < 1)
        {
            Debug.Log("코스트가 부족합니다.");
            return;
        }
        int opponentId = playerId == 1 ? 2 : 1; // 상대 플레이어의 ID 계산
        GameObject myParticipation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Participation").gameObject;
        GameObject opponentParticipation = GameObject.FindGameObjectsWithTag("Player" + opponentId)[0]
            .transform.Find("Participation").gameObject;
        
        var opponentCards = opponentParticipation.GetComponentsInChildren<CharacterCard>();
        var myCards = myParticipation.GetComponentsInChildren<CharacterCard>();
        
        if (myCards.Length == 0) return;
        if (opponentCards.Length > 0)
        {
            int damage = 1;
            if (myCards[0].skillCard is MilesEdgeworth)
            {
                if (myCards[0].hp < 3)
                {
                    damage += 1;
                }
            }
            var targetCard = opponentCards[0];
            Debug.Log($"Player{playerId}이(가) {targetCard.name}에게 일반 공격을 했습니다.");
            Client.Instance.SendUpdateCostEvent(1);
            Client.Instance.SendAttackEvent(playerId, damage);
        }
        else
        {
            Debug.Log("상대방의 Participation에 카드가 없습니다.");
        }
    }
    
    public void SkillAttack()
    {
        if (Client.Instance.GetCurrentTurnPlayerID() != Client.Instance.GetPlayerID())
        {
            Debug.Log("내 턴이 아닙니다. 스킬을 사용할 수 없습니다.");
            return;
        }
        // 현재 턴에 따라 자신의 Participation과 상대방의 Participation 결정
        int playerId = Client.Instance.GetPlayerID();
        int opponentId = playerId == 1 ? 2 : 1; // 상대 플레이어의 ID 계산
        GameObject myParticipation = GameObject.FindGameObjectsWithTag("Player" + playerId)[0]
            .transform.Find("Participation").gameObject;
        GameObject opponentParticipation = GameObject.FindGameObjectsWithTag("Player" + opponentId)[0]
            .transform.Find("Participation").gameObject;

        // 자신의 Participation에서 첫 번째 카드 가져오기
        var opponentCards = opponentParticipation.GetComponentsInChildren<CharacterCard>();
        var myCards = myParticipation.GetComponentsInChildren<CharacterCard>();
        
        if (myCards.Length == 0) return;

        var activeCard = myCards[0];
        var activeSkill = activeCard.skillCard;
        if (activeSkill == null)
        {
            Debug.Log("스킬이 없는 카드입니다.");
            return;
        }
        // 상대방의 Participation에서 첫 번째 카드 가져오기
        if (opponentCards.Length == 0)
        {
            Debug.Log("상대방의 Participation에 카드가 없습니다.");
            return;
        }
        
        int myCost = Client.Instance.GetPlayerID() == 1 ? int.Parse(player1cost.text) : int.Parse(player2cost.text);
        
        if (activeCard.skillCard.skillCost > myCost)
        {
            Debug.Log("코스트가 부족합니다.");
            return;
        }
        
        var targetCard = opponentCards[0];
        Debug.Log($"Player{Client.Instance.GetPlayerID()}이(가) {targetCard.name}에게 스킬 공격을 했습니다.");
        Client.Instance.SendSkillEvent(playerId, activeCard.skillCard.skillType, activeCard.skillCard.skillCost);
        Client.Instance.SendUpdateCostEvent(activeCard.skillCard.skillCost);
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
    
    public void UpdateTurnUI()
    {
        // UI 업데이트
        bool isMyTurn = Client.Instance.GetPlayerID() == Client.Instance.GetCurrentTurnPlayerID();
        turnEndButton.interactable = isMyTurn;
        _turnC += 1;
        if (_turnC >= 2)
        {
            _turnC = 0;
            _Round += 1;
            Client.Instance.SendAddCostEvent(Client.Instance.GetPlayerID(), 1);
        }
        Debug.Log(isMyTurn ? "내 턴입니다." : "상대방의 턴입니다.");
    }

    public void UpdateCostUI()
    {
        if (Client.Instance.GetPlayerID() == 0) return;
        
        player1cost.text = Client.Instance.PlayerCosts[1].ToString();
        player2cost.text = Client.Instance.PlayerCosts[2].ToString();
    }
    
    public void UpdateCost(Utilities.CostEvent costEvent)
    {
        Client.Instance.PlayerCosts[costEvent.playerID] -= costEvent.usedCost;
        UpdateCostUI();
    }
    
    public void AddCost(Utilities.CostAddEvent costAddEvent)
    {
        Client.Instance.PlayerCosts[costAddEvent.playerID] += costAddEvent.addCost;
        UpdateCostUI();
    }
}
