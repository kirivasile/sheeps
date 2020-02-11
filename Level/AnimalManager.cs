using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;
using Zenject;

namespace Sheeps.Level {
    public class AnimalManager : IFixedTickable {
        Settings _settings;
        Animal.Factory _animalFactory;
        TileMap _tileMap;

        bool _started;

        List<Animal> _animals = new List<Animal>();

        public AnimalManager(Animal.Factory animalFactory, TileMap tileMap) {
            _animalFactory = animalFactory;
            _tileMap = tileMap;
        }

        public void Start(Settings settings) {
            _settings = settings;
            _started = true;

            for (int i = _animals.Count; i < _settings.numAnimals; ++i) {
                SpawnNext();
            }
        }

        public void Pause() {
            _started = false;
        }

        public void Continue() {
            _started = true;
        }

        public void FixedTick() {
            if (_started) {
                
                for (int i = 0; i < _animals.Count; ++i) {
                    _animals[i].FixedTick();
                }
            }
        }

        public void SpawnNext() {
            _tileMap.SampleAnimalFoodPositions(out var animalPosition, out var foodTile);
            
            Color color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            Animal animal = _animalFactory.Create(_settings.animalSpeed);
            animal.Position = animalPosition;
            animal.FoodTile = foodTile;
        
            animal.ObjColor = color;

            _animals.Add(animal);
        }

        public SaveData Save() {
            var animalData = new List<Animal.SaveData>();
            foreach (Animal animal in _animals) {
                animalData.Add(animal.Save());
            }

            return new SaveData() {
                animals = animalData,
                numAnimals = _settings.numAnimals,
                animalSpeed = _settings.animalSpeed
            };
        }

        public void Load(SaveData saveData) {
            _settings = new Settings() {
                numAnimals = saveData.numAnimals,
                animalSpeed = saveData.animalSpeed
            };

            for (int i = 0; i < saveData.numAnimals; ++i) {
                Animal animal = _animalFactory.Create(_settings.animalSpeed);
                animal.Load(saveData.animals[i]);

                _animals.Add(animal);
            }
        }

        [Serializable]
        public class SaveData {
            public List<Animal.SaveData> animals;
            public int numAnimals;
            public float animalSpeed;
        }

        [Serializable]
        public class Settings {
            public int numAnimals;
            public float animalSpeed; 
        }
    }
}