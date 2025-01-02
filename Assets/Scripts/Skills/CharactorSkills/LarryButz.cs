public class LarryButz : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상

    protected override void OnSkill()
    {
        targetCharactor.hp -= 3;
        // 자신 제외 모든 캐릭터 hp 업 네트워크 구성 완료 이후 수정
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}