using UnityEngine.Serialization;

public class MiaFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter;

    public override void OnSkill()
    {
        // myCharactor.hp += 1;
        // targetCharactor.hp -= 2;
        // 네트워크 관련 로직 구성 이후, 다시 구성
        throw new System.NotImplementedException();
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}