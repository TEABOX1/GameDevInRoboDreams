using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public interface IInteractableService : IService
    {
        void AddInteractable(Collider collider, IInteractable interactable);
        void RemoveInteractable(Collider collider, IInteractable interactable);
        bool CanInteract(Collider collider, out IInteractable interactable);
    }
}