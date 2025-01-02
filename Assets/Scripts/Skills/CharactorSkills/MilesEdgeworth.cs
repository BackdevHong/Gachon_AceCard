using UnityEngine;

public class MilesEdgeworth : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상
    public bool isTriggered = false;

    public override void OnSkill()
    {
        targetCharactor.hp -= 2;
        if (myCharactor.hp > 3 && isTriggered) return;
        myCharactor.attack += 2;
        isTriggered = true;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}