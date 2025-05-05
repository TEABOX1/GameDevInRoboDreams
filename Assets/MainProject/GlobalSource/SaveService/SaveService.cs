using System;
using System.IO;
using UnityEngine;

namespace GlobalSource
{
    public class SaveService : GlobalMonoServiceBase, ISaveService
    {
        [SerializeField] private string _savePath;
        [SerializeField] private string _saveFile;
        
        private SaveData _saveData;
        
        public ref SaveData SaveData => ref _saveData;
        
        public override Type Type { get; } = typeof(ISaveService);

        protected override void Awake()
        {
            base.Awake();
            
            LoadAll();
        }

        protected override void OnDestroy()
        {
            SaveAll();
            
            base.OnDestroy();
        }

        public void SaveAll()
        {
            string path = Path.Combine(Application.persistentDataPath, _savePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, _saveFile);
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            
            File.WriteAllText(filePath, JsonUtility.ToJson(_saveData, true));
        }

        public void LoadAll()
        {
            string path = Path.Combine(Application.persistentDataPath, _savePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }
            string filePath = Path.Combine(path, _saveFile);
            if (!File.Exists(filePath))
            {
                _saveData.soundData = SoundSaveData.Default;
                return;
            }
            
            _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath));
        }
    }
}