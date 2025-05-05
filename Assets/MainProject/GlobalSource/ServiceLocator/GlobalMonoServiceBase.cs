using System;
using UnityEngine;

namespace GlobalSource
{
    [DefaultExecutionOrder(-20)]
    public abstract class GlobalMonoServiceBase : MonoServiceBase
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}