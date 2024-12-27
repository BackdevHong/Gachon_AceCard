public class KarmaMai : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상
    
    public override void OnSkill()
    {
        targetCharactor.hp -= 2;
        targetCharactor.hp -= 2;

        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}