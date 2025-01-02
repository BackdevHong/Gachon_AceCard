public class MiaFey : Skill
{
    public CharactorCard myCharactor;
    public CharactorCard targetCharactor;

    protected override void OnSkill()
    {
        // myCharactor.hp += 1;
        // targetCharactor.hp -= 2;
        // 네트워크 관련 로직 구성 이후, 다시 구성
        throw new System.NotImplementedException();
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}