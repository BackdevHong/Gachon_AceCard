using System.Collections.Generic;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class MiaFey : Skill
{
    [FormerlySerializedAs("myCharactor")] public CharacterCard myCharacter;
    [FormerlySerializedAs("targetCharactor")] public CharacterCard targetCharacter;
    
    private void Awake()
    {
        skillType = SkillType.All; // 스킬 타입 설정
        skillCost = 2; // 스킬 비용 설정
    }
    
    public override void OnSkill()
    {
        int myPid = Client.Instance.GetPlayerID();
        int enemyPid = myPid == 1 ? 2 : 1;
        
        List<CharacterCard> myCards = GameManager.Instance.GetAllCards(myPid);
        List<CharacterCard> enemyCards = GameManager.Instance.GetAllCards(enemyPid);
        
        foreach (var card in myCards)
        {
            card.Heal(2);
        }
        
        foreach (var card in enemyCards)
        {
            card.TakeDamage(2);
        }
    }
    
    public override void UpdateText()
    {
        myCharacter.hpText.text = myCharacter.hp.ToString();
        targetCharacter.hpText.text = targetCharacter.hp.ToString();
    } 
}