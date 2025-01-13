using System;
using System.Collections.Generic;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

    public List<GameObject> SelectedCards { get; private set; } = new List<GameObject>();

    private int _turnC = -1;
    private int _Round = 0;

    // 행동카드 관련 변수
    private bool _freeSwitch = false;
    private bool _reduceCost = false;
    private bool _freeSkillCost = false;
    public bool appendSkillDamage = false;
    private bool _addNormalAttackDamage = false;
    public bool usedHpCard = false;
    private bool _freeAttack = false;

    private int _addedCost = 0;
    private int _healTurn = 0;
    private int _healUser = 0;

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
    }

    public void Connect()
    {
        Client.Instance.Connect("localhost", 7777);
    }

    public void StartServer()
    {
        Server.Instance.CreateServer();
    }

    public void ReplaceCardsRandomly()
    {
        if (SelectedCards.Count != 3)
        {
            Debug.LogError("새 프리팹 리스트는 정확히 3개여야 합니다.");
            return;
        }

        // Preparation과 Participation 영역 가져오기
        GameObject preparationArea = Client.Instance.GetPlayerID() == 1 ? player1Preparation : player2Preparation;
        GameObject participationArea = Client.Instance.GetPlayerID() == 1
            ? GameObject.FindWithTag("Player1").transform.Find("Participation").gameObject
            : GameObject.FindWithTag("Player2").transform.Find("Participation").gameObject;

        // 기존 Preparation 카드들 가져오기
        var existingPreparationCards = preparationArea.GetComponentsInChildren<CharacterCard>();
        if (existingPreparationCards.Length != 2)
        {
            Debug.LogError("Preparation 영역에 기존 카드가 2개가 있어야 합니다.");
            return;
        }

        // 기존 Participation 카드 가져오기
        var existingParticipationCards = participationArea.GetComponentsInChildren<CharacterCard>();
        if (existingParticipationCards.Length != 1)
        {
            Debug.LogError("Participation 영역에 기존 카드가 1개가 있어야 합니다.");
            return;
        }

        // 새 프리팹을 랜덤으로 섞기
        List<GameObject> shuffledPrefabs = SelectedCards.OrderBy(card => Random.value).ToList();

        // Preparation 카드 교체
        for (int i = 0; i < existingPreparationCards.Length; i++)
        {
            Vector3 oldPosition = existingPreparationCards[i].transform.localPosition;
            Vector3 oldScale = existingPreparationCards[i].transform.localScale;
            Transform parent = existingPreparationCards[i].transform.parent;

            Destroy(existingPreparationCards[i].gameObject);

            GameObject newCard = Instantiate(shuffledPrefabs[i], parent);
            newCard.transform.localPosition = oldPosition;
            newCard.transform.localScale = oldScale; // 크기 초기화
        }

        // Participation 카드 교체
        Vector3 participationPosition = existingParticipationCards[0].transform.localPosition;
        Transform participationParent = existingParticipationCards[0].transform.parent;
        Vector3 participationScale = existingParticipationCards[0].transform.localScale;

        Destroy(existingParticipationCards[0].gameObject);

        GameObject newParticipationCard = Instantiate(shuffledPrefabs[2], participationParent);
        newParticipationCard.transform.localPosition = participationPosition;
        newParticipationCard.transform.localScale = participationScale; // 크기 초기화

        Debug.Log("Preparation과 Participation 카드가 새 프리팹으로 랜덤하게 교체되었습니다.");
    }

    public void SetSelectedCards(List<GameObject> cards)
    {
        SelectedCards = cards;
        Debug.Log("GameManager에 선택된 카드가 저장되었습니다.");
    }

    public void StartSwitch()
    {
        if (Client.Instance.GetCurrentTurnPlayerID() != Client.Instance.GetPlayerID()) return;
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
            if (_freeSwitch)
            {
                return;
            }

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

            if (_addNormalAttackDamage)
            {
                damage += 1;
            }
            var targetCard = opponentCards[0];
            Debug.Log($"Player{playerId}이(가) {targetCard.name}에게 일반 공격을 했습니다.");
            Client.Instance.SendAttackEvent(playerId, damage);
            if (_reduceCost)
            {
                _reduceCost = false;
                return;
            }
            Client.Instance.SendUpdateCostEvent(1);
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

        if (!Client.Instance.PlayerCosts.TryGetValue(playerId, out int playerCost))
        {
            Debug.LogError($"Player {playerId} not found in _playerCosts.");
            return;
        }

        Debug.Log($"Player {playerId} Current Cost: {playerCost}");

        if (activeCard.skillCard.skillCost > playerCost)
        {
            Debug.Log("코스트가 부족합니다.");
            return;
        }

        var targetCard = opponentCards[0];
        Debug.Log($"Player{Client.Instance.GetPlayerID()}이(가) {targetCard.name}에게 스킬 공격을 했습니다.");
        Client.Instance.SendSkillEvent(playerId, activeCard.skillCard.skillType, activeCard.skillCard.skillCost);
        Debug.Log(activeCard.skillCard.skillCost);
        if (_freeSkillCost)
        {
            _freeSkillCost = false;
        }
        else
        {
            Client.Instance.SendUpdateCostEvent(activeCard.skillCard.skillCost);
        }
    }

    public void OnActionUse(ActionType type)
    {
        switch (type)
        {
            case ActionType.FreeSwitch:
                _freeSwitch = true;
                break;
            case ActionType.ReduceCost:
                _reduceCost = true;
                break;
            case ActionType.FreeSkillCost:
                _freeSkillCost = true;
                break;
            case ActionType.AddNormalAttackDamage:
                _addNormalAttackDamage = true;
                break;
            case ActionType.AddSkillDamage:
                appendSkillDamage = true;
                break;
            case ActionType.FreeAttack:
                _freeAttack = true;
                break;
        }
    }

    public void OnActionUse(ActionType type, int value)
    {
        switch (type)
        {
            case ActionType.AddCost:
                _addedCost = value;
                Client.Instance.SendAddCostEvent(Client.Instance.GetPlayerID(), value);
                break;
            case ActionType.AddOneHp:
                Client.Instance.SendAddHpEvent(Client.Instance.GetPlayerID(), value, 1);
                usedHpCard = true;
                break;
            case ActionType.AddLongHp:
                Client.Instance.SendAddHpEvent(Client.Instance.GetPlayerID(), value, 1);
                usedHpCard = true;
                _healTurn = 2;
                _healUser = Client.Instance.GetPlayerID();
                break;
        }
    }

    public void UpdateTurnUI()
    {
        // UI 업데이트
        bool isMyTurn = Client.Instance.GetPlayerID() == Client.Instance.GetCurrentTurnPlayerID();
        turnEndButton.interactable = isMyTurn;
        if (_freeSwitch || _reduceCost || usedHpCard)
        {
            _freeSwitch = false;
            _reduceCost = false;
            usedHpCard = false;
        }

        if (_healTurn < 0)
        {
            _healTurn--;
            Client.Instance.SendAddHpEvent(_healUser, 1, 1);
        }

        _turnC += 1;
        if (_turnC >= 2)
        {
            _turnC = 0;
            _Round += 1;
            Client.Instance.SendAddCostEvent(Client.Instance.GetPlayerID(), 1);
        }

        if (_addedCost != 0)
        {
            Client.Instance.SendUpdateCostEvent(_addedCost);
            _addedCost = 0;
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
        if (Client.Instance.PlayerCosts[costAddEvent.playerID] > 8)
        {
            return;
        }
        Client.Instance.PlayerCosts[costAddEvent.playerID] += costAddEvent.addCost;
        UpdateCostUI();
    }

    public List<CharacterCard> GetAllCard(int playerID)
    {
        List<CharacterCard> allCards = new List<CharacterCard>();

        // 플레이어의 Preparation과 Participation 오브젝트 가져오기
        GameObject playerObject = GameObject.FindWithTag("Player" + playerID);
        if (playerObject == null)
        {
            Debug.LogError($"Player {playerID} 오브젝트를 찾을 수 없습니다.");
            return allCards;
        }

        GameObject preparation = playerObject.transform.Find("Preparation")?.gameObject;
        GameObject participation = playerObject.transform.Find("Participation")?.gameObject;

        if (preparation == null || participation == null)
        {
            Debug.LogError($"Player {playerID}의 Preparation 또는 Participation 영역을 찾을 수 없습니다.");
            return allCards;
        }

        // Preparation에서 모든 CharacterCard 수집
        var preparationCards = preparation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(preparationCards);

        // Participation에서 모든 CharacterCard 수집
        var participationCards = participation.GetComponentsInChildren<CharacterCard>();
        allCards.AddRange(participationCards);

        return allCards;
    }

    public CharacterCard GetMainCharacterCard(int playerID)
    {
        // 플레이어 오브젝트 가져오기
        GameObject playerObject = GameObject.FindWithTag("Player" + playerID);
        if (playerObject == null)
        {
            Debug.LogError($"Player {playerID} 오브젝트를 찾을 수 없습니다.");
            return null;
        }

        // Participation 영역 가져오기
        GameObject participation = playerObject.transform.Find("Participation")?.gameObject;
        if (participation == null)
        {
            Debug.LogError($"Player {playerID}의 Participation 영역을 찾을 수 없습니다.");
            return null;
        }

        // Participation에서 첫 번째 CharacterCard 반환
        CharacterCard[] participationCards = participation.GetComponentsInChildren<CharacterCard>();
        if (participationCards.Length > 0)
        {
            return participationCards[0]; // 첫 번째 카드를 메인 캐릭터 카드로 반환
        }

        Debug.LogError($"Player {playerID}의 Participation에 캐릭터 카드가 없습니다.");
        return null; // Participation에 카드가 없는 경우
    }


    public void HealOneCard(int playerID, int addHp)
    {
        CharacterCard card = GetMainCharacterCard(playerID);
        card.Heal(addHp);
    }

    public void HealCard(int playerID, int addHp)
    {
        List<CharacterCard> card = GetAllCard(playerID);
        foreach (var characterCard in card)
        {
            characterCard.Heal(addHp);
        }
    }
}