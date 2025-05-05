namespace GlobalSource
{
    public interface ISoundService : IService
    {
        void SetVolume(SoundType type, float volume);
        float GetVolume(SoundType type);
    }
}