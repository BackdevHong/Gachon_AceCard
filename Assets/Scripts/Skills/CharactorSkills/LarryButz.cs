using UnityEngine.Serialization;

public class LarryButz : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상

    public override void OnSkill()
    {
        targetCharacter.hp -= 3;
        // 자신 제외 모든 캐릭터 hp 업 네트워크 구성 완료 이후 수정
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}