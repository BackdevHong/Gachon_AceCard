using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class WinstonPayne : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    private void Awake()
    {
        skillType = SkillType.One; // 스킬 타입 설정
        skillCost = 2; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        System.Random random = new System.Random();
        int chance = random.Next(0, 2); // 0 또는 1을 반환 (50% 확률)

        if (chance == 0)
        {
            // 상대에게 3의 피해를 줌
            SelectedTarget.TakeDamage(4);
            if (GameManager.Instance.appendSkillDamage)
            {
                SelectedTarget.TakeDamage(3);
            }
            Debug.Log("상대에게 4의 피해를 주었습니다.");
        }
        else
        {
            // 본인에게 2의 피해를 줌
            myCharacter.TakeDamage(4);
            Debug.Log("본인에게 4의 피해를 입었습니다.");
        }
    }
}