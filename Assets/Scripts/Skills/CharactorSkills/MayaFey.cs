using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class MayaFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 6; // 스킬 비용 설정
    }
    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        myCharacter.Heal(4);
        
        List<CharacterCard> allCards = GameManager.Instance.GetAllCards(Client.Instance.GetPlayerID());
        foreach (var card in allCards)
        {
            if (card == myCharacter) continue;
            card.Heal(2); // 아군 모두에게 회복
        }
    }
}