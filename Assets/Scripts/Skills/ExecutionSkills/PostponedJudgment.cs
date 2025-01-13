using Skills;
using UnityEngine;

public class PostponedJudgment : Skill
{
    public override void OnSkill()
    {
        ActionCard actionCard = GetComponent<ActionCard>();

        if (actionCard == null)
        {
            Debug.Log("Error: No action card attached");
            return;
        }

        if (GameManager.Instance.usedHpCard)
        {
            return;
        }
        
        Client.Instance.SendUpdateCostEvent(actionCard.cost);
        GameManager.Instance.OnActionUse(ActionType.AddLongHp, 1);
        Destroy(this);
    }
}