using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    private Skill activeSkillCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveSkillCard(Skill skillCard)
    {
        activeSkillCard = skillCard;
        Debug.Log($"활성화된 스킬 카드: {skillCard.name}");
    }

    public Skill GetActiveSkillCard()
    {
        return activeSkillCard;
    }

    public void ResetActiveSkillCard()
    {
        activeSkillCard = null;
        Debug.Log("스킬 카드 초기화 완료.");
    }
}