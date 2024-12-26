public class MiaFey : Skill
{
    public CharactorCard myCharactor;
    public CharactorCard targetCharactor;
    
    public void OnSkill()
    {
        myCharactor.hp += 1;
        targetCharactor.hp -= 2;
        throw new System.NotImplementedException();
    }
}