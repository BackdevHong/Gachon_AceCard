using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class TurnManager : NetworkBehaviour
{
    [SyncVar] private int currentTurnPlayer = 0; // 현재 턴인 플레이어 ID
    [SyncVar] private float remainingTime = 60f; // 남은 시간
    private Coroutine timerCoroutine; // 제한 시간 타이머 코루틴

    public Text timerText; // 클라이언트에서 제한 시간 표시
    public Text turnText; // 클라이언트에서 턴 표시

    private void Start()
    {
        if (isServer)
        {
            // 서버에서만 타이머를 시작
            StartNewTurn();
        }
    }

    // 새로운 턴 시작
    [Server]
    private void StartNewTurn()
    {
        currentTurnPlayer = (currentTurnPlayer + 1) % 2; // 플레이어 교대 (0 -> 1 -> 0)
        remainingTime = 60f; // 60초로 제한 시간 초기화

        // 모든 클라이언트에 턴과 제한 시간 동기화
        RpcUpdateTurnData(currentTurnPlayer, remainingTime);

        // 타이머 시작
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TurnTimer());
    }

    // 제한 시간 타이머
    [Server]
    private IEnumerator TurnTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

            // 클라이언트에 남은 시간 동기화
            RpcUpdateTurnData(currentTurnPlayer, remainingTime);
        }

        // 제한 시간이 끝나면 턴 종료
        EndTurn();
    }

    // 턴 종료
    [Server]
    public void EndTurn()
    {
        Debug.Log($"플레이어 {currentTurnPlayer}의 턴이 종료되었습니다.");
        StartNewTurn(); // 다음 턴 시작
    }

    // 클라이언트에 턴과 제한 시간 동기화
    [ClientRpc]
    public void RpcUpdateTurnData(int turnPlayer, float time)
    {
        currentTurnPlayer = turnPlayer;
        remainingTime = time;

        // UI 업데이트
        if (timerText != null)
        {
            timerText.text = $"남은 시간: {Mathf.CeilToInt(remainingTime)}초";
        }
        if (turnText != null)
        {
            turnText.text = $"플레이어 {currentTurnPlayer + 1}의 턴";
        }
    }

    // 클라이언트에서 턴 종료 요청
    [Command]
    public void CmdEndTurn()
    {
        if (isServer)
        {
            EndTurn(); // 서버에서 턴 종료 처리
        }
    }
}
