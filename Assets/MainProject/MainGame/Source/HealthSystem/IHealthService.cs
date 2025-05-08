using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public interface IHealthService : IService
    {
        IHealth this[Collider collider] { get; }

        void AddCharacter(IHealth character);
        void RemoveCharacter(IHealth character);
        bool GetHealth(Collider collider, out IHealth health);
    }
}
