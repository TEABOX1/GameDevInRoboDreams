using System.Collections;
using UnityEngine;

namespace Boot
{
    public class Boot : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return null;
            //ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }
    }
}