using Skills;
using UnityEngine.Serialization;

public class PheonixWright : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;
    private void Awake()
    {
        skillType = SkillType.One; // 스킬 타입 설정
        skillCost = 3; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        myCharacter.Heal(1);
        SelectedTarget.TakeDamage(3);
        if (GameManager.Instance.appendSkillDamage)
        {
            SelectedTarget.TakeDamage(3);
        }
    }
}