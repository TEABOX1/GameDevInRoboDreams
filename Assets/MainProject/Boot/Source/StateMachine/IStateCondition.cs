namespace Boot
{
    public interface IStateCondition
    {
        byte State { get; }
        
        bool Invoke();
    }
}