using UnityEngine;

namespace MainGame
{
    public interface IPlayerdar
    {
        //TargetableBase CurrentTarget { get; } // ������� �������� ������ TargetableBase
        bool HasTarget { get; }
        bool SeesTarget { get; }
        Vector3 LastTargetPosition { get; }

        void LookAround();
    }
}
