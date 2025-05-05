namespace GlobalSource
{
    public interface IStateCondition
    {
        byte State { get; }
        
        bool Invoke();
    }
}