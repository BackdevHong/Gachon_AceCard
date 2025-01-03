using UnityEngine;
using UnityEngine.Serialization;

public class WinstonPayne : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상

    public override void OnSkill()
    {
        System.Random random = new System.Random();
        int chance = random.Next(0, 2); // 0 또는 1을 반환 (50% 확률)

        if (chance == 0)
        {
            // 상대에게 3의 피해를 줌
            targetCharacter.hp -= 3;
            Debug.Log("상대에게 3의 피해를 주었습니다.");
        }
        else
        {
            // 본인에게 2의 피해를 줌
            myCharacter.hp -= 2;
            Debug.Log("본인에게 2의 피해를 입었습니다.");
        }
        
        UpdateText();
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}