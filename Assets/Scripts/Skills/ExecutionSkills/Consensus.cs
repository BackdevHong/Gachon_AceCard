using UnityEngine;

public class Consensus : Skill
{
    public CharactorCard targetCharactor; // 스킬 대상
    public bool isTriggered = false;

    public override void OnSkill()
    {
        targetCharactor.hp += 1;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    }
}
