using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    public class BootForPlayerTest : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public IEnumerator Start()
        {
            yield return null;
            SceneManager.LoadScene(_sceneName);
        }
    }
}