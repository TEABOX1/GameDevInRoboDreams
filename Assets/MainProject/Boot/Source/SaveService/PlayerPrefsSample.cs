using TMPro;
using UnityEngine;

namespace Boot
{
    public class PlayerPrefsSample : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private string _key;

        public void Load()
        {
            _inputField.text = PlayerPrefs.GetString(_key);
        }

        public void Save()
        {
            PlayerPrefs.SetString(_key, _inputField.text);
            PlayerPrefs.Save();
        }
    }
}