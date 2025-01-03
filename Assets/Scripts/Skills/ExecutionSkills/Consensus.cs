using UnityEngine;
using UnityEngine.Serialization;

public class Consensus : Skill
{
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상
    public bool isTriggered = false;

    public override void OnSkill()
    {
        targetCharacter.hp += 1;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    }
}
