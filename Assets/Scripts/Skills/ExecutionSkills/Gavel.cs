using System;
using Skills;
using UnityEngine;
using UnityEngine.Serialization;

public class Gavel : Skill
{
    public override void OnSkill()
    {       
        ActionCard actionCard = GetComponent<ActionCard>();
        
        if (actionCard == null)
        {
            Debug.Log("Error: No action card attached");
            return;
        }
        
        Client.Instance.SendUpdateCostEvent(actionCard.cost);
        GameManager.Instance.OnActionUse(ActionType.AddNormalAttackDamage);
        Destroy(this);
    }
}
