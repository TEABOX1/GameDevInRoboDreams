using GlobalSource;

namespace Boot
{
    public class Cutscene : GameStateBase
    {
        public Cutscene(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, Scenes scene) : base(stateMachine, stateId, sceneManager, scene)
        {
        }

        public override void Dispose()
        {
        }
    }
}