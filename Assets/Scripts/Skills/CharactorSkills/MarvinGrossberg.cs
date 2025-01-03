using UnityEngine.Serialization;

public class MarvinGrossberg : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상

    public override void OnSkill()
    {
        targetCharacter.hp -= 2;
        // 네트워크 관련 로직 구성 이후, 모든 플레이어 대상으로 한 스킬 구현
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}