using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public interface IPlayerService : IService
    {
        TargetableBase Player { get; }
        bool IsPlayer(Collider collider);
    }
}
