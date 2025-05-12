using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        public event Action<IInteractable> onDestroy;
        
        [SerializeField] protected GameObject rootObject;
        [SerializeField] protected Collider collider;
        // [SerializeField] protected BillboardBase tooltip;

        private Transform _transform;
        
        public Vector3 Position => _transform.position;
        
        protected virtual void Awake()
        {
            ServiceLocator.Instance.GetService<IInteractableService>().AddInteractable(collider, this);
            // tooltip.SetCamera(ServiceLocator.Instance.GetService<ICameraService>().Camera);
            // Highlight(false);
            _transform = collider.transform;
        }

        protected virtual void OnDestroy()
        {
            // Highlight(false);
            onDestroy?.Invoke(this);
            ServiceLocator.Instance?.GetService<IInteractableService>()?.RemoveInteractable(collider, this);
        }

        public virtual void Interact()
        {
            Destroy(rootObject);
        }

        // public virtual void Highlight(bool active)
        // {
        //     tooltip.gameObject.SetActive(active);
        // }
    }
}