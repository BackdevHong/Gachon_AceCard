using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class MiaFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;
    
    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 6; // 스킬 비용 설정
    }
    
    public override void OnSkill()
    {
        int myPid = Client.Instance.GetPlayerID();
        int enemyPid = myPid == 1 ? 2 : 1;
        
        List<CharacterCard> myCards = GameUIManager.Instance.GetAllCards(myPid);
        List<CharacterCard> enemyCards = GameUIManager.Instance.GetAllCards(enemyPid);
        
        foreach (var card in myCards)
        {
            card.Heal(2);
        }
        
        foreach (var card in enemyCards)
        {
            card.TakeDamage(1);
            if (GameManager.Instance.appendSkillDamage)
            {
                card.TakeDamage(3);
            }
        }
    }
}