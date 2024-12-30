using UnityEngine;

public class Skill : MonoBehaviour
{
    
    public bool onSelectionNeed = false; // 카드 선택 여부 플래그
    protected bool isTargetSelected = false; // 대상이 선택되었는지 여부
    
    public virtual void OnSkill()
    {
        
    }

    public virtual void UpdateText()
    {
        
    }
    
    private void OnMouseDown()
    {
        if (onSelectionNeed && !isTargetSelected)
        {
            Debug.Log("대상을 먼저 선택해야 합니다.");
            return;
        }
        
        // 프리팹 클릭 시 OnSkill 실행
        OnSkill();
        Debug.Log("스킬 실행 완료");
    }
    
    public void SelectTarget(CharactorCard target)
    {
        if (onSelectionNeed)
        {
            isTargetSelected = true;
            Debug.Log($"대상 {target.name}이(가) 선택되었습니다.");
        }
    }
 }
