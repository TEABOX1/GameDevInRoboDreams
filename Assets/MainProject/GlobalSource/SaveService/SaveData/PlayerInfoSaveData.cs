using System;
using MainGame;

namespace GlobalSource
{
    [Serializable]
    public struct PlayerInfoSaveData
    {
        public int DebugSaveInfo;
        public QuestData[] questData;
    }
}