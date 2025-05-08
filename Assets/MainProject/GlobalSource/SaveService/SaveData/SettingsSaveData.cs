using System;

namespace GlobalSource
{
    [Serializable]
    public struct SettingsSaveData
    {
        public SoundSaveData soundData;
        public LocalizationSaveData localizationData;
    }
}