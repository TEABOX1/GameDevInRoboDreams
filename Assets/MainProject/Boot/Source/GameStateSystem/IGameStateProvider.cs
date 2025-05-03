using System;

namespace Boot
{
    public interface IGameStateProvider : IService
    {
        event Action<GameState> OnGameStateChanged;
        
        GameState GameState { get; }
        
        void SetGameState(GameState gameState);
    }
}