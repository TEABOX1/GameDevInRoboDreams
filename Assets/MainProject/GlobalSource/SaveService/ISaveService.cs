namespace GlobalSource
{
    public interface ISaveService : IService
    {
        void SaveAll();
        void LoadAll();
        void ResetSaveData();
        ref SaveData SaveData { get; }
    }
}