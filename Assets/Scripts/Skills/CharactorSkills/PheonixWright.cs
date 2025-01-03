using UnityEngine.Serialization;

public class PheonixWright : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter;

    public override void OnSkill()
    {
        myCharacter.hp += 1;
        targetCharacter.hp -= 2;
        throw new System.NotImplementedException();
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}