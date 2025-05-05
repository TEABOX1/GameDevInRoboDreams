using System;

namespace GlobalSource
{
    public interface IState : IDisposable
    {
        byte StateId { get; }
        
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }
}