using Skills;
using UnityEngine;

public class Investigation : Skill
{
    public override void OnSkill()
    {
        ActionCard actionCard = GetComponent<ActionCard>();

        if (actionCard == null)
        {
            Debug.Log("Error: No action card attached");
            return;
        }
        
        GameManager.Instance.OnActionUse(ActionType.FreeAttack);
        Destroy(this);
    }
}