using TMPro;
using UnityEngine;

public class ActionCard : MonoBehaviour
{
    public int cost;
    public TMP_Text costText; // 공격력 표시 TextMeshPro
    
    public int GetCost()
    {
        return cost;
    }
    
    private void Start()
    {
        costText.text = cost.ToString();
    }
}
