using System;

namespace GlobalSource
{
    [Serializable]
    public struct SaveData
    {
        public bool isNewGame;
        public SoundSaveData soundData;
        public LocalizationSaveData localizationData;
    }
}