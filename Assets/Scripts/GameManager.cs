using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1Participation; // Player1의 Participation 오브젝트
    public GameObject player1Preparation;   // Player1의 Preparation 오브젝트
    public GameObject player2Participation; // Player2의 Participation 오브젝트
    public GameObject player2Preparation;   // Player2의 Preparation 오브젝트

    private CharactorCard selectedPreparationCard; // 선택된 Preparation 카드
    public bool isSwitching = false; // 교체 모드 활성화 여부
    private bool isPlayer1Turn = true; // Player1 턴인지 여부
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

    public void StartSwitch()
    {
        isSwitching = true; // 교체 모드 활성화
        Debug.Log(isPlayer1Turn ? "Player1 교체 모드 활성화" : "Player2 교체 모드 활성화");
    }

    public void SelectPreparationCard(CharactorCard card)
    {
        if (!isSwitching)
        {
            Debug.Log("교체 모드가 아닙니다.");
            return;
        }

        // 현재 턴에 따라 올바른 Preparation 영역 확인
        GameObject currentPreparation = isPlayer1Turn ? player1Preparation : player2Preparation;

        if (card.transform.parent != currentPreparation.transform)
        {
            Debug.Log("현재 턴의 Preparation에 있는 카드만 선택할 수 있습니다.");
            return;
        }

        selectedPreparationCard = card;
        Debug.Log($"{card.name} 카드를 선택했습니다. Participation에 있는 카드와 교체합니다.");
        SwitchParticipationCard();
    }

    public void SwitchParticipationCard()
    {
        if (!isSwitching || selectedPreparationCard == null)
        {
            Debug.Log("교체 모드가 비활성화되었거나 Preparation 카드가 선택되지 않았습니다.");
            return;
        }

        // 현재 턴에 따라 Participation 영역 결정
        GameObject currentParticipation = isPlayer1Turn ? player1Participation : player2Participation;

        var participationCards = currentParticipation.GetComponentsInChildren<CharactorCard>();
        if (participationCards.Length == 0)
        {
            Debug.Log("Participation에 교체할 카드가 없습니다.");
            return;
        }

        var participationCard = participationCards[0]; // 첫 번째 Participation 카드

        // 두 카드의 현재 위치 저장
        Vector3 preparationPosition = selectedPreparationCard.transform.position;
        Vector3 participationPosition = participationCard.transform.position;

        // 부모 교환
        Transform preparationParent = selectedPreparationCard.transform.parent;
        Transform participationParent = participationCard.transform.parent;

        selectedPreparationCard.transform.SetParent(participationParent);
        participationCard.transform.SetParent(preparationParent);

        // 위치 교환
        selectedPreparationCard.transform.position = participationPosition;
        participationCard.transform.position = preparationPosition;

        Debug.Log($"{selectedPreparationCard.name}과 {participationCard.name}의 위치가 교환되었습니다.");

        // 초기화
        selectedPreparationCard = null;
        isSwitching = false;
    }

    // public void NormalAttack()
    // {
    //     GameObject myParticipation = isPlayer1Turn ? player1Participation : player2Participation;
    //     GameObject opponentParticipation = isPlayer1Turn ? player2Participation : player1Participation;
    //     var opponentCards = opponentParticipation.GetComponentsInChildren<CharactorCard>();
    //     var myCards = myParticipation.GetComponentsInChildren<CharactorCard>();
    //     
    //     if (myCards.Length == 0) return;
    //     if (opponentCards.Length > 0)
    //     {
    //         var targetCard = opponentCards[0];
    //         var myCard = myCards[0];
    //         int damage = 1;
    //
    //         GameEvent attackEvent = new GameEvent
    //         {
    //             eventType = "Attack",
    //             attacker = myCard,
    //             target = targetCard,
    //             damage = damage
    //         };
    //
    //         TCPClientManager.Instance.SendMessageToServer(JsonUtility.ToJson(attackEvent));
    //     }
    //     else
    //     {
    //         Debug.Log("상대방의 Participation에 카드가 없습니다.");
    //     }
    // }
    //
    // public void SkillAttack()
    // {
    //     GameObject myParticipation = isPlayer1Turn ? player1Participation : player2Participation;
    //     GameObject opponentParticipation = isPlayer1Turn ? player2Participation : player1Participation;
    //
    //     var myCards = myParticipation.GetComponentsInChildren<CharactorCard>();
    //     if (myCards.Length == 0) return;
    //
    //     var opponentCards = opponentParticipation.GetComponentsInChildren<CharactorCard>();
    //     if (opponentCards.Length == 0) return;
    //
    //     GameEvent skillEvent = new GameEvent
    //     {
    //         eventType = "Skill",
    //         attacker = myCards[0],
    //         target = opponentCards[0],
    //     };
    //
    //     TCPClientManager.Instance.SendMessageToServer(JsonUtility.ToJson(skillEvent));
    // }
    //
    // public void HandleGameEvent(GameEvent gameEvent)
    // {
    //     switch (gameEvent.eventType)
    //     {
    //         case "Attack":
    //             Debug.Log($"{gameEvent.attacker}이(가) {gameEvent.target}에게 {gameEvent.damage}의 피해를 입혔습니다.");
    //             break;
    //         case "Skill":
    //             Debug.Log($"{gameEvent.attacker}이(가) {gameEvent.target}에게 스킬 사용");
    //             gameEvent.attacker.skillCard.OnSkill();
    //             break;
    //         case "Switch":
    //             Debug.Log($"{gameEvent.switchFrom}이(가) {gameEvent.switchTo}와 교체되었습니다.");
    //             break;
    //     }
    // }
}
