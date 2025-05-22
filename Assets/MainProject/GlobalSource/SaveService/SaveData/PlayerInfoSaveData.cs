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
        public float PlayerRotationY;
        public int HealthValue;
        public QuestData[] questData;

        public static readonly PlayerInfoSaveData Default = new PlayerInfoSaveData()
        {
            PlayerPosition = new Vector3(29f, 1f, 65f),
            PlayerRotationY = 0f,
            HealthValue = 100,
            questData = null
        }; // CHANGED
    }
}