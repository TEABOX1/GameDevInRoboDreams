using UnityEngine;

namespace MainGame
{
    public interface IPlayerdar
    {
        //TargetableBase CurrentTarget { get; } // Потрібно створити скрипт TargetableBase
        bool HasTarget { get; }
        bool SeesTarget { get; }
        Vector3 LastTargetPosition { get; }

        void LookAround();
    }
}
