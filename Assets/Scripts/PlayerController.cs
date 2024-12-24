using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SyncVar]
    public string playerName; // 플레이어 이름

    public GameObject chatCanvas;  // 채팅 UI Canvas
    public TMP_Text chatText;      // 채팅 메시지 TextMeshPro

    private float chatDisplayTime = 3f; // 채팅 메시지 표시 시간
    private Coroutine hideChatCoroutine;
    public float moveSpeed = 1f;

    private void Start()
    {
        if (isLocalPlayer)
        {
            playerName = $"Player_{netId}";
        }

        // 채팅 UI 초기 비활성화
        if (chatCanvas != null)
        {
            chatCanvas.SetActive(false);
        }
    }

    // 서버에서 클라이언트로 채팅 메시지 전송
    [Command]
    public void CmdSendChatMessage(string message)
    {
        RpcReceiveChatMessage($"{playerName}: {message}");
    }

    // 모든 클라이언트에 채팅 메시지 동기화
    [ClientRpc]
    public void RpcReceiveChatMessage(string message)
    {
        if (chatText != null)
        {
            chatText.text = message;

            // 채팅 UI 활성화
            if (chatCanvas != null)
            {
                chatCanvas.SetActive(true);

                // 기존 코루틴 종료 후 새로 시작
                if (hideChatCoroutine != null)
                {
                    StopCoroutine(hideChatCoroutine);
                }
                hideChatCoroutine = StartCoroutine(HideChatAfterDelay());
            }
        }
    }

    // 일정 시간이 지난 후 채팅 UI 숨김
    private IEnumerator HideChatAfterDelay()
    {
        yield return new WaitForSeconds(chatDisplayTime);
        if (chatCanvas != null)
        {
            chatCanvas.SetActive(false);
        }
    }

    // 로컬 플레이어에서 채팅 메시지 입력 처리
    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Return)) // Enter 키로 메시지 입력
        {
            string inputMessage = "Hello!"; // 임시 메시지 (UI와 연동 가능)
            CmdSendChatMessage(inputMessage);
        }

        HandleMovement();
    }
    
    private void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f; // 앞으로 이동
        if (Input.GetKey(KeyCode.S)) moveZ = -1f; // 뒤로 이동
        if (Input.GetKey(KeyCode.A)) moveX = -1f; // 왼쪽 이동
        if (Input.GetKey(KeyCode.D)) moveX = 1f; // 오른쪽 이동

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World); // 로컬 플레이어 이동
    }
}