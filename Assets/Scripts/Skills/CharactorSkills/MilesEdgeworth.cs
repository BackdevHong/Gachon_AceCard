using UnityEngine;
using UnityEngine.Serialization;

public class MilesEdgeworth : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    

    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        SelectedTarget.TakeDamage(2); // 대상에게 데미지
        if (myCharacter.hp <= 3)
        {
            SelectedTarget.TakeDamage(2); // 대상에게 데미지
        }
    }
}