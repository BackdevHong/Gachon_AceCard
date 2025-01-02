using UnityEngine;

public class Skill : MonoBehaviour
{
    public bool onSelectionNeed = false; // 카드 선택 여부 플래그
    protected CharactorCard SelectedTarget = null; // 선택된 대상 카드

    protected virtual void OnSkill()
    {
        // 스킬 실행 로직
    }

    public virtual void UpdateText()
    {
        
    }

    public void SelectTarget(CharactorCard target)
    {
        SelectedTarget = target;
        Debug.Log($"스킬 대상 설정: {target.name}");
        ExecuteSkill();
    }

    private void ExecuteSkill()
    {
        if (SelectedTarget == null)
        {
            Debug.LogError("타겟이 설정되지 않았습니다.");
            return;
        }

        OnSkill(); // 스킬 실행
        ResetSkill();
    }

    private void ResetSkill()
    {
        SelectedTarget = null;
    }
}