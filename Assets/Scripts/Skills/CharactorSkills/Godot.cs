﻿using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class Godot : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자

    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 3; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        SelectedTarget.TakeDamage(4);
        if (GameManager.Instance.appendSkillDamage)
        {
            SelectedTarget.TakeDamage(3);
        }
    }
}