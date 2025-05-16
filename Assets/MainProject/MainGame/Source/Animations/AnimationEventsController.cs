using System;
using GlobalSource;
using UnityEngine;

namespace MainGame
{
    public class AnimationEventsController : MonoBehaviour
    {
        public event Action OnDeathFinished;

        public void DeathFinished()
        {
            Debug.Log("end death animation");
            OnDeathFinished?.Invoke();
        }
    }
}