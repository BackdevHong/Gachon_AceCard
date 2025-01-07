using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MarvinGrossberg : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter; // 스킬 대상

    public override void OnSkill()
    {
        if (targetCharacter == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        targetCharacter.TakeDamage(1); // 대상에게 데미지
        List<CharacterCard> myCharacterList = GameManager.Instance.GetAllCards(Client.Instance.GetPlayerID());
        foreach (var card in myCharacterList)
        {
            card.Heal(2); // 아군 모두에게 데미지
        }
    }
}