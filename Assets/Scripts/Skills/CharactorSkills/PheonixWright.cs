public class PheonixWright : Skill
{
    public CharactorCard myCharactor;
    public CharactorCard targetCharactor;

    protected override void OnSkill()
    {
        myCharactor.hp += 1;
        targetCharactor.hp -= 2;
        throw new System.NotImplementedException();
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}