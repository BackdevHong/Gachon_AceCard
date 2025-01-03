using UnityEngine;
using UnityEngine.Serialization;

public class Gavel : Skill
{
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상
    public bool isTriggered = false;

    public override void OnSkill()
    {
        targetCharacter.attack += 1;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        targetCharacter.attackText.text = targetCharacter.attack.ToString();
    }
}
