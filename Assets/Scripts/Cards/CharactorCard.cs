using System;
using TMPro;
using UnityEngine;

public class CharactorCard : MonoBehaviour
{
    public int hp; // 체력
    public int attack = 0; // 공격력
    
    public TMP_Text hpText; // 체력 표시 TextMeshPro
    public TMP_Text attackText; // 공격력 표시 TextMeshPro
    
    public int plusHp = 0; // 추가체력 기본값 0
    public int plusAttack = 0; // 추가 공격력 기본값 0
    public int plusCost = 0; // 추가 코스트 기본값 0
    public bool regen = false; // 체력 재생 여부 기본값 false

    public bool isSkillUsed = false;

    public int GetHp()
    {
        return hp;
    }
    
    public int GetAttack()
    {
        return attack;
    }
    
    public void SkillUse()
    {
        isSkillUsed = true;
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
