public class MarvinGrossberg : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상
    
    public override void OnSkill()
    {
        targetCharactor.hp -= 2;
        // 네트워크 관련 로직 구성 이후, 모든 플레이어 대상으로 한 스킬 구현
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}