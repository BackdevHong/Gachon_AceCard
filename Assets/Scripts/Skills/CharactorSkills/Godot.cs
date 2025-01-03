using UnityEngine.Serialization;

public class Godot : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상

    public override void OnSkill()
    {
        targetCharacter.hp -= 3;
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}