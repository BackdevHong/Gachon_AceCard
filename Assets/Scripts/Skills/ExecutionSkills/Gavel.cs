using UnityEngine;

public class Gavel : Skill
{
    public CharactorCard targetCharactor; // 스킬 대상
    public bool isTriggered = false;

    protected override void OnSkill()
    {
        targetCharactor.attack += 1;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        targetCharactor.attackText.text = targetCharactor.attack.ToString();
    }
}
