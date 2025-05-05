using System;
using GlobalSource;

namespace Boot
{
    public interface IGameStateProvider : IService
    {
        event Action<GameState> OnGameStateChanged;
        
        GameState GameState { get; }
        
        void SetGameState(GameState gameState);
    }
}