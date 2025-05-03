using System;
using UnityEngine;

namespace Boot
{
    public interface ISceneManager : IService
    {
        event Action<AsyncOperation> onSceneLoad;
        
        void SetScene(Scenes scene);
        void OnSceneLoad(AsyncOperation operation);
    }
}