using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class KarmaMai : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    private void Awake()
    {
        skillType = SkillType.One; // 스킬 타입 설정
        skillCost = 5; // 스킬 비용 설정
    }
    
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        SelectedTarget.TakeDamage(3); // 대상에게 데미지
        SelectedTarget.TakeDamage(3); // 대상에게 데미지
        if (GameManager.Instance.appendSkillDamage)
        {
            SelectedTarget.TakeDamage(3);
        }
    }
}