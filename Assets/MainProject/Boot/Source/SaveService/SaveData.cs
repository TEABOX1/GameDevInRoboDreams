using System;

namespace Boot
{
    [Serializable]
    public struct SaveData
    {
        public SoundSaveData soundData;
        public LocalizationSaveData localizationData;
    }
}