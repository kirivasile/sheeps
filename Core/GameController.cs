using System;
using UnityEngine;
using UnityEngine.Assertions;
using Sheeps.Level;


namespace Sheeps.Core {
    public class GameController {
        AnimalManager _animalManager;
        TileMap _map;
        Settings _settings;

        bool _isPaused;

        public GameController(AnimalManager animalManager, TileMap map) {
            _animalManager = animalManager;
            _map = map;
        }

        public void SetSettings(Settings settings) {
            _settings = settings;
        }

        public void StartSimulation() {
            Assert.IsNotNull(_settings);
            _map.Start(_settings.mapSize);
            _animalManager.Start(LoadAnimalManagerSettings());
            _isPaused = false;
        }

        public void Pause() {
            _animalManager.Pause();
        }

        public void Continue() {
            _animalManager.Continue();
        }


        AnimalManager.Settings LoadAnimalManagerSettings() {
            return new AnimalManager.Settings(){
                numAnimals = _settings.numAnimals,
                animalSpeed = _settings.animalSpeed
            };
        }

        public SaveData Save() {
            return new SaveData() {
                animalManager = _animalManager.Save(),
                tileMap = _map.Save()
            };
        }

        public void Load(SaveData saveData) {
            _settings.mapSize = saveData.tileMap.mapSize;
            _settings.numAnimals = saveData.animalManager.numAnimals;
            _settings.animalSpeed = saveData.animalManager.animalSpeed;
            _animalManager.Load(saveData.animalManager);
        }

        [Serializable]
        public class SaveData {
            public AnimalManager.SaveData animalManager;
            public TileMap.SaveData tileMap;
        }

        public class Settings {
            public int mapSize;
            public int numAnimals;
            public float animalSpeed;
        }
    }
}