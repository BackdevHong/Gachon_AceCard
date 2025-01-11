using Skills;
using UnityEngine;

public class Argument : Skill
{
    public override void OnSkill()
    {
        ActionCard actionCard = GetComponent<ActionCard>();

        if (actionCard == null)
        {
            Debug.Log("Error: No action card attached");
            return;
        }
        
        GameManager.Instance.OnActionUse(ActionType.FreeSkillCost);
        Destroy(this);
    }
}