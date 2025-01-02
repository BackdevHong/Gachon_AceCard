using System;
using UnityEngine;

public class Karma : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상
    public String selectedType;

    protected override void OnSkill()
    {
        myCharactor.hp -= 2;
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
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}