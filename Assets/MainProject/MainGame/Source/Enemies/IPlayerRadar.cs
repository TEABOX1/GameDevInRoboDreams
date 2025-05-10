using UnityEngine;

namespace MainGame
{
    public interface IPlayerRadar
    {
        Transform CurrentTarget { get; }
        IPlayerService PlayerService { get; }
        bool HasTarget { get; }
        bool SeesTarget { get; }
        Vector3 LastTargetPosition { get; }

        void LookAround();
    }
}
