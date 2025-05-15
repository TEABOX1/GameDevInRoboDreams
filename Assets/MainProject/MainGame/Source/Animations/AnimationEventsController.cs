using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class AnimationEventsController : MonoServiceBase
    {
        public override Type Type { get; } = typeof(AnimationEventsController);

        public event Action OnDeathFinished;

        public void DeathFinished()
        {
            Debug.Log("end death animation");
            OnDeathFinished?.Invoke();
        }
    }
}