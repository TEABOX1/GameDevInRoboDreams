using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class AnimationEventsController : MonoServiceBase
    {
        public override Type Type { get; } = typeof(AnimationEventsController);

        public event Action OnStartDealDamage;
        public event Action OnStopDealDamage;
        public event Action OnAttackAnimationEnd;

        public void StartDealDamage()
        {
            OnStartDealDamage?.Invoke();
        }

        public void StopDealDamage()
        {
            OnStopDealDamage?.Invoke();
        }

        public void AttackAnimationEnd()
        {
            OnAttackAnimationEnd?.Invoke();
        }
    }
}