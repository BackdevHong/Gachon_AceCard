using UnityEngine;
using UnityEngine.Serialization;

public class DickGumshoe : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    public string type;
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }

        myCharacter.hp -= 2; // 스킬 비용
        SelectedTarget.TakeDamage(5); // 대상에게 데미지
        Debug.Log($"{myCharacter.name}이(가) {SelectedTarget.name}에게 스킬을 사용했습니다.");
    }

    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        SelectedTarget.hpText.text = SelectedTarget.hp.ToString();
    }
}