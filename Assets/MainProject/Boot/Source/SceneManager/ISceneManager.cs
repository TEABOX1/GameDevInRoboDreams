using System;
using UnityEngine;
using GlobalSource;

namespace Boot
{
    public interface ISceneManager : IService
    {
        event Action<AsyncOperation> onSceneLoad;
        
        void SetScene(Scenes scene);
        void ReloadCurrentScene();
        void OnSceneLoad(AsyncOperation operation);
    }
}