using System;
using TMPro;
using UnityEngine;

public class CharactorCard : MonoBehaviour
{
    public int hp; // 체력
    public int attack = 0; // 공격력
    public int skillCost = 0;
    public Skill skillCard;
    
    public TMP_Text hpText; // 체력 표시 TextMeshPro
    public TMP_Text attackText; // 공격력 표시 TextMeshPro

    public int GetHp()
    {
        return hp;
    }
    
    public int GetAttack()
    {
        return attack;
    }

    private void Start()
    {
        hpText.text = GetHp().ToString();
        attackText.text = GetAttack().ToString();
    }
    
    private void UpdateUI()
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
        }
    }

    // 공격했을 때
    public void Attack(CharactorCard target)
    {
        Debug.Log($"{name}이(가) {target.name}을(를) 공격합니다.");
        target.TakeDamage(attack);
    }
}
