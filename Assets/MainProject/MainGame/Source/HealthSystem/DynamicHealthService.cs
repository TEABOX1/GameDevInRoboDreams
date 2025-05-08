using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    [DefaultExecutionOrder(-10)]
    public class DynamicHealthService : DynamicHealthSystem, IHealthService
    {
        public Type Type { get; } = typeof(IHealthService);

        protected virtual void Awake()
        {
            ServiceLocator.Instance.AddService(this);
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.RemoveService(this);
        }
    }
}
