using System;

namespace GlobalSource
{
    [Serializable]
    public struct SaveData
    {
        public SettingsSaveData settingsData;
        public PlayerInfoSaveData playerInfoData;
    }
}