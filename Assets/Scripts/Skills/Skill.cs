using UnityEngine;

public class Skill : MonoBehaviour
{
    public virtual void OnSkill()
    {
        
    }

    public virtual void UpdateText()
    {
        
    }
    
    private void OnMouseDown()
    {
        // 프리팹 클릭 시 OnSkill 실행
        OnSkill();
        Debug.Log("실행 완료");
    }
 }
