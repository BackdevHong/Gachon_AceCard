using UnityEngine;

public class DickGumshoe : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자

    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }

        myCharactor.hp -= 2; // 스킬 비용
        SelectedTarget.TakeDamage(5); // 대상에게 데미지
        Debug.Log($"{myCharactor.name}이(가) {SelectedTarget.name}에게 스킬을 사용했습니다.");
    }

    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        SelectedTarget.hpText.text = SelectedTarget.hp.ToString();
    }
}