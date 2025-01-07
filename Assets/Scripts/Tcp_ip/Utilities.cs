using System;
using UnityEngine;

public class Utilities
{
    [Serializable]
    public class AttackEvent
    {
        public int attackPlayerID; // 공격자 ID
        public int damage;     // 데미지
        public int cost;       // 코스트
    }
    
    [Serializable]
    public class SkillEvent
    {
        public int attackPlayerID;
        public string type; // 공격자 ID
        public int cost; // 코스트
    }
    
    [Serializable]
    public class SwitchEvent
    {
        public int playerID;
        public int switchIndexed; // 공격자 ID
    }
    
    [Serializable]
    public class TurnEvent
    {
        public int currentTurnPlayerID; // 현재 턴을 가진 플레이어 ID
    }
}