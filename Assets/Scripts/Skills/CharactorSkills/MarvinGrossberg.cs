﻿using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class MarvinGrossberg : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 6; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        SelectedTarget.TakeDamage(1); // 대상에게 데미지
        if (GameManager.Instance.appendSkillDamage)
        {
            SelectedTarget.TakeDamage(3);
        }
        List<CharacterCard> myCharacterList = GameUIManager.Instance.GetAllCards(Client.Instance.GetPlayerID());
        foreach (var card in myCharacterList)
        {
            card.Heal(3); // 아군 모두에게 힐
        }
    }
}