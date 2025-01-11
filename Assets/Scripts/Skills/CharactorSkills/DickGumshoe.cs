using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class DickGumshoe : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    
    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 4; // 스킬 비용 설정
    }
    
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }

        myCharacter.TakeDamage(1); // 스킬 비용
        int enemyPid = Client.Instance.GetPlayerID() == 1 ? 2 : 1; 
        List<CharacterCard> allCards = GameUIManager.Instance.GetAllCards(enemyPid);
        foreach (var card in allCards)
        {
            card.TakeDamage(2); // 적군 모두에게 데미지
            if (GameManager.Instance.appendSkillDamage)
            {
                card.TakeDamage(3);
            }
        }
        Debug.Log($"{myCharacter.name}이(가) {SelectedTarget.name}에게 스킬을 사용했습니다.");
    }

    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        SelectedTarget.hpText.text = SelectedTarget.hp.ToString();
    }
}