using Skills;
using UnityEngine;

public class Costs : Skill
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
        GameManager.Instance.OnActionUse(ActionType.AddCost, 2);
        Destroy(this);
    }
}