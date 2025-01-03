using UnityEngine;
using UnityEngine.Serialization;

public class MilesEdgeworth : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상
    public bool isTriggered = false;

    public override void OnSkill()
    {
        targetCharacter.hp -= 2;
        if (myCharacter.hp > 3 && isTriggered) return;
        myCharacter.attack += 2;
        isTriggered = true;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}