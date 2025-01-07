using System;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class Karma : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    public String selectedType;
    private void Awake()
    {
        skillType = SkillType.One; // 스킬 타입 설정
        skillCost = 3; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        myCharacter.hp -= 2;
        if (selectedType == "card")
        {
            // 카드 뽑는 로직
        } else if (selectedType == "cost")
        {
            // Cost 관련 로직
        }
    }

    public void onSelect()
    {
        // 선택지 관련 로직 
    }
}