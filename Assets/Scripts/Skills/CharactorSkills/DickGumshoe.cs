using UnityEngine;

public class DickGumshoe : Skill
{
    public CharactorCard myCharactor; // 스킬 사용자
    public CharactorCard targetCharactor; // 스킬 대상
    
    public override void OnSkill()
    {
        if (onSelectionNeed && !isTargetSelected)
        {
            Debug.Log("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }

        myCharactor.hp -= 2;
        targetCharactor.hp -= 5; // 예시: 대상에게 데미지를 줌
        UpdateText();
    }

    public override void UpdateText()
    {
        myCharactor.hpText.text = myCharactor.hp.ToString();
        targetCharactor.hpText.text = targetCharactor.hp.ToString();
    }
}