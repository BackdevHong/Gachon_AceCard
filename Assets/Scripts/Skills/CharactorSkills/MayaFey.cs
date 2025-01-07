using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MayaFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter; // 스킬 사용자

    public override void OnSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("스킬 발동 조건이 충족되지 않았습니다.");
            return;
        }
        
        myCharacter.Heal(3);
        
        List<CharacterCard> allCards = GameManager.Instance.GetAllCards(Client.Instance.GetPlayerID());
        foreach (var card in allCards)
        {
            if (card != myCharacter) continue;
            card.Heal(2); // 아군 모두에게 회복
        }
    }
}