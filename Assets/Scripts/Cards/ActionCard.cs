using UnityEngine;

public class ActionCard : MonoBehaviour
{
    public int cost;
    public string effect; // 효과 종류 (예: "Attack", etc...)
    public int value; // 효과 값 (예: +3 공격력, +5 방어력 등)
    
    public int GetCost()
    {
        return cost;
    }
    

    public void UseActionCard(CharactorCard targetCharacter)
    {
        switch (effect)
        {
            case "Attack":
                targetCharacter.attack += value;
                Debug.Log($"{targetCharacter.name}의 공격력이 {value}만큼 증가했습니다.");
                break;

            default:
                Debug.LogWarning("알 수 없는 효과입니다.");
                break;
        }
        // Destroy(gameObject); // 카드 사용 후 제거
    }
}
