using System;
using System.Collections.Generic;
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
    
    [Serializable]
    public class CostEvent
    {
        public int playerID;
        public int usedCost;
    }
    
    [Serializable]
    public class CostAddEvent
    {
        public int playerID;
        public int addCost;
    }
    
    [Serializable]
    public class HPAddEvent
    {
        public int playerID;
        public int addHp;
        public int type; // 1 = 혼자, 2 = 다같이
    }
    
    [Serializable]
    public class CostData
    {
        public int PlayerID;
        public int Cost;

        public CostData(int playerID, int cost)
        {
            PlayerID = playerID;
            Cost = cost;
        }
    }
}