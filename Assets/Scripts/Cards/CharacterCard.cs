using System;
using Skills;
using TMPro;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    public int hp; // 체력
    public int attack = 0; // 공격력

    // 스킬 파라미터
    public Skill skillCard;

    public string uniqueID; // 고유 ID
    public TMP_Text hpText; // 체력 표시 TextMeshPro
    public TMP_Text attackText; // 공격력 표시 TextMeshPro
    
    private bool isDead = false; // 캐릭터가 죽었는지 여부

    private void Awake()
    {
        // 고유 ID 생성 (동일한 값으로 재사용 가능)
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
    private void Start()
    {
        UpdateUI();
    }


    public void UpdateUI()
    {
        hpText.text = $"{hp}";
        attackText.text = $"{attack}";
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 상태라면 데미지를 받지 않음

        hp -= damage;
        UpdateUI();

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        hp += heal;
        UpdateUI();
    }
    
    public void Die()
    {
        isDead = true;
        hp = 0;
        UpdateUI();
        gameObject.SetActive(false); // 오브젝트를 비활성화
        Debug.Log($"{name} 캐릭터가 쓰러졌습니다.");
    }
    
    public void Revive()
    {
        if (!isDead) return; // 죽은 상태가 아니면 부활하지 않음

        isDead = false;
        hp = 10; // 체력을 10으로 초기화
        gameObject.SetActive(true); // 오브젝트를 활성화
        UpdateUI();
        Debug.Log($"{name} 캐릭터가 부활했습니다! 체력: {hp}");
    }

    private void OnMouseDown()
    {
        /*Debug.Log("test1");
        //if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            //Debug.Log("우클릭 잘 눌림");

            // 기존 팝업 닫기
            if (activePopup != null && activePopup != popup)
            {
                activePopup.SetActive(false);
            }

            // 현재 팝업 활성화
            if (popup != null)
            {
                popup.SetActive(true);
                activePopup = popup; // 활성화된 팝업 업데이트
                Debug.Log("우클릭 잘 눌림");

            }
        }*/

        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            if (ObjectSelector.Instance != null)
            {
                ObjectSelector.Instance.SelectObject(gameObject);
            }
            else
            {
                Debug.Log("ObjectSelector Instance가 존재하지 않습니다.");
            }
        }

        if (GameManager.Instance)
        {
            // GameManager의 SelectParticipationCard 호출
            GameManager.Instance.SelectPreparationCard(this);
        }
        else
        {
            Debug.Log($"{name} 카드가 클릭되었습니다.");
        }
    }
}