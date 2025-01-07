using System;
using Skills;
using TMPro;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    public int hp; // 체력
    public int attack = 0; // 공격력
    
    // 스킬 파라미터
    public Skill skillCard;
    
    public string uniqueID; // 고유 ID
    public TMP_Text hpText; // 체력 표시 TextMeshPro
    public TMP_Text attackText; // 공격력 표시 TextMeshPro
    private void Awake()
    {
        // 고유 ID 생성 (동일한 값으로 재사용 가능)
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        hpText.text = $"{hp}";
        attackText.text = $"{attack}";
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        UpdateUI();
        if (hp <= 0)
        {
            Debug.Log($"{name} 캐릭터가 쓰러졌습니다.");
            Destroy(gameObject); // 카드 제거
        }
    }
    
    public void Heal(int heal)
    {
        hp += heal;
        UpdateUI();
    }
    
    private void OnMouseDown()
    {
        if (GameManager.Instance.isSwitching)
        {
            // GameManager의 SelectParticipationCard 호출
            GameManager.Instance.SelectPreparationCard(this);
        }
        else
        {
            Debug.Log($"{name} 카드가 클릭되었습니다.");
        }
    }
}