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
    public GameObject popup; // 캐릭터 오브젝트 안에 있는 팝업
    private static GameObject activePopup; // 현재 활성화된 팝업 (전역적으로 관리)


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
        hp -= damage;

        UpdateUI();
        if (hp <= 0)
        {
            Debug.Log($"{name} 캐릭터가 쓰러졌습니다.");
            Destroy(gameObject); // 카드 제거
        }
    }

    public void Heal(int heal)
    {
        hp += heal;
        UpdateUI();
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
    public void ClosePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
            if (activePopup == popup)
            {
                activePopup = null;
            }
        }
    }
}