using System;
using UnityEngine;

namespace Boot
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