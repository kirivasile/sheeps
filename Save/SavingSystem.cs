using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zenject;
using Sheeps.Core;

namespace Sheeps.Save {
    public class SavingSystem {
        GameController _gameController;

        const string fileName = "game.sav";

        readonly string _filePath;

        public SavingSystem(GameController gameController) {
            _gameController = gameController;

            _filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Save() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_filePath, FileMode.Create);
            
            GameController.SaveData saveData = _gameController.Save();

            formatter.Serialize(stream, saveData);
            stream.Close();
        }

        public bool Load() {
            if (File.Exists(_filePath)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(_filePath, FileMode.Open);

                GameController.SaveData data = formatter.Deserialize(stream) as GameController.SaveData;
                stream.Close();

                _gameController.Load(data);

                return true;
            } else {
                Debug.Log($"Save file under path {_filePath} not found");
                return false;
            }
        }
    }
}