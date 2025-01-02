using UnityEngine;

public class WinstonPayne : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상

    protected override void OnSkill()
    {
        System.Random random = new System.Random();
        int chance = random.Next(0, 2); // 0 또는 1을 반환 (50% 확률)

        if (chance == 0)
        {
            // 상대에게 3의 피해를 줌
            targetCharactor.hp -= 3;
            Debug.Log("상대에게 3의 피해를 주었습니다.");
        }
        else
        {
            // 본인에게 2의 피해를 줌
            myCharactor.hp -= 2;
            Debug.Log("본인에게 2의 피해를 입었습니다.");
        }
        
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    } 
}