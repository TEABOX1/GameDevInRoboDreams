using GlobalSource;

namespace Boot
{
    public class Gameplay : GameStateBase
    {
        public Gameplay(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, Scenes scene) : base(stateMachine, stateId, sceneManager, scene)
        {
        }

        public override void Dispose()
        {
        }
    }
}