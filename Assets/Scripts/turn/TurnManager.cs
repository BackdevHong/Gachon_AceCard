using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class TurnManager : NetworkBehaviour
{
    [SyncVar] private int currentTurnPlayer = 0; // ���� ���� �÷��̾� ID
    [SyncVar] private float remainingTime = 60f; // ���� �ð�
    private Coroutine timerCoroutine; // ���� �ð� Ÿ�̸� �ڷ�ƾ

    public Text timerText; // Ŭ���̾�Ʈ���� ���� �ð� ǥ��
    public Text turnText; // Ŭ���̾�Ʈ���� �� ǥ��

    private void Start()
    {
        if (isServer)
        {
            // ���������� Ÿ�̸Ӹ� ����
            StartNewTurn();
        }
    }

    // ���ο� �� ����
    [Server]
    private void StartNewTurn()
    {
        currentTurnPlayer = (currentTurnPlayer + 1) % 2; // �÷��̾� ���� (0 -> 1 -> 0)
        remainingTime = 60f; // 60�ʷ� ���� �ð� �ʱ�ȭ

        // ��� Ŭ���̾�Ʈ�� �ϰ� ���� �ð� ����ȭ
        RpcUpdateTurnData(currentTurnPlayer, remainingTime);

        // Ÿ�̸� ����
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TurnTimer());
    }

    // ���� �ð� Ÿ�̸�
    [Server]
    private IEnumerator TurnTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

            // Ŭ���̾�Ʈ�� ���� �ð� ����ȭ
            RpcUpdateTurnData(currentTurnPlayer, remainingTime);
        }

        // ���� �ð��� ������ �� ����
        EndTurn();
    }

    // �� ����
    [Server]
    public void EndTurn()
    {
        Debug.Log($"�÷��̾� {currentTurnPlayer}�� ���� ����Ǿ����ϴ�.");
        StartNewTurn(); // ���� �� ����
    }

    // Ŭ���̾�Ʈ�� �ϰ� ���� �ð� ����ȭ
    [ClientRpc]
    public void RpcUpdateTurnData(int turnPlayer, float time)
    {
        currentTurnPlayer = turnPlayer;
        remainingTime = time;

        // UI ������Ʈ
        if (timerText != null)
        {
            timerText.text = $"���� �ð�: {Mathf.CeilToInt(remainingTime)}��";
        }
        if (turnText != null)
        {
            turnText.text = $"�÷��̾� {currentTurnPlayer + 1}�� ��";
        }
    }

    // Ŭ���̾�Ʈ���� �� ���� ��û
    [Command]
    public void CmdEndTurn()
    {
        if (isServer)
        {
            EndTurn(); // �������� �� ���� ó��
        }
    }
}
