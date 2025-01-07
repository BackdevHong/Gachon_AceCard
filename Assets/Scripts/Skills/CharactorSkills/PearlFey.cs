using UnityEngine;
using UnityEngine.Serialization;

public class PearlFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;

    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        SelectedTarget.TakeDamage(1);
        myCharacter.Heal(1);
    }
}