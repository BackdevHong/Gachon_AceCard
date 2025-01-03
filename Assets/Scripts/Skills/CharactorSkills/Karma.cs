using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Karma : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상
    public String selectedType;

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
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}