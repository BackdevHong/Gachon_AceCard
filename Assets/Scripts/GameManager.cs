using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1Participation; // Player1의 Participation 오브젝트
    public GameObject player1Preparation; // Player1의 Preparation 오브젝트
    public GameObject player2Participation; // Player2의 Participation 오브젝트
    public GameObject player2Preparation; // Player2의 Preparation 오브젝트

    private CharacterCard selectedPreparationCard; // 선택된 Preparation 카드
    public bool isSwitching = false; // 교체 모드 활성화 여부
    public static GameManager Instance;

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

    public void StartSwitch()
    {
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
        }
    }

    public void NormalAttack()
    {
        int playerId = Client.Instance.GetPlayerID();
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
            var targetCard = opponentCards[0];
            Debug.Log($"Player{playerId}이(가) {targetCard.name}에게 일반 공격을 했습니다.");
            Client.Instance.SendAttackEvent(playerId, damage);
        }
        else
        {
            Debug.Log("상대방의 Participation에 카드가 없습니다.");
        }
    }
    
    public void SkillAttack()
    {
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
        
        var targetCard = opponentCards[0];
        Debug.Log($"Player{Client.Instance.GetPlayerID()}이(가) {targetCard.name}에게 스킬 공격을 했습니다.");
        Client.Instance.SendSkillEvent(playerId, "One");
    }
}
