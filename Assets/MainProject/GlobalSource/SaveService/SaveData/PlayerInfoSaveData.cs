using System;
using MainGame;
using UnityEngine;

namespace GlobalSource
{
    [Serializable]
    public struct PlayerInfoSaveData
    {
        public int DebugSaveInfo;
        public Vector3 PlayerPosition;
        // public float PlayerRotationY;
        public int HealthValue;
        public QuestData[] questData;
    }
}