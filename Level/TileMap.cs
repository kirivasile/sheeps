using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sheeps.Level {
    public class TileMap {
        Settings _settings;
        CameraHandler _camera;
        Transform _map;

        LinkedList<Tile> _availableAnimalPositions;
        LinkedList<Tile> _availableFoodPositions;

        public const string MapId = "Map";
        
        public TileMap(
            Settings settings, 
            CameraHandler camera,

            [Inject(Id = TileMap.MapId)]
            Transform map
        ) {
            _settings = settings;
            _camera = camera;
            _map = map;
        }

        int MapSize {
            get {
                return _settings.mapSize;
            }
            set {
                _settings.mapSize = value;

                _map.localScale = new Vector3(value, value, 1f);
                _map.transform.position = new Vector3(
                    (float)value / 2 - 0.5f,
                    (float)value / 2 - 0.5f
                );

                _camera.ChangeMapSize(value);
            }
        }
        
        public void Start(int mapSize) {
            MapSize = mapSize;

            _availableAnimalPositions = GetShuffledList(_settings.mapSize);
            _availableFoodPositions = GetShuffledList(_settings.mapSize);
        }
        

        public void SampleAnimalFoodPositions(out Vector3 animalPos, out Tile foodTile) {
            animalPos = Vector3.zero;
            Tile animalPosition = _availableAnimalPositions.First.Value;
            _availableAnimalPositions.RemoveFirst();
            animalPos = new Vector3(
                animalPosition.Position.x,
                animalPosition.Position.y
            );

            foodTile = SampleFoodPosition(animalPos);
        }

        public Tile SampleFoodPosition(Vector3 animalPosition) {
            LinkedListNode<Tile> it = _availableFoodPositions.First;
            while (it != null) {
                Tile tile = it.Value;
                float distance = tile.Distance(animalPosition);
                if (!tile.IsFoodAttached && 0 < distance && distance < _settings.maxFoodDistance) {
                    return tile;
                }
                it = it.Next;
            }

            return null;
        }

        public List<Vector3> GetAvailableDirections(Vector3 position) {
            List<Vector3> availableDirections = new List<Vector3>() {
                Vector3.left, Vector3.up, Vector3.down, Vector3.right
            };

            int tileX = (int)Mathf.Round(position.x);
            int tileY = (int)Mathf.Round(position.y);

            if (tileX == 0) availableDirections.Remove(Vector3.left);
            if (tileX == _settings.mapSize) availableDirections.Remove(Vector3.right);
            if (tileY == 0) availableDirections.Remove(Vector3.down);
            if (tileY == _settings.mapSize) availableDirections.Remove(Vector3.up);

            return availableDirections;
        }

        LinkedList<Tile> GetShuffledList(int size) {
            List<Tile> buf = new List<Tile>();

            for (int i = 0; i < size; ++i) {
                for (int j = 0; j < size; ++j) {
                    buf.Add(new Tile(new Vector3(i, j)));
                }
            }

            // Shuffling
            for (int i = 0; i < buf.Count; ++i) {
                var temp = buf[i];
                int randomIdx = Random.Range(i, buf.Count);
                buf[i] = buf[randomIdx];
                buf[randomIdx] = temp;
            }

            return new LinkedList<Tile>(buf);
        }

        public SaveData Save() {
            return new SaveData() {
                mapSize = _settings.mapSize
            };
        }

        public void Load(SaveData saveData) {
            MapSize = saveData.mapSize;
        }

        [Serializable]
        public class SaveData {
            public int mapSize;
        }

        [Serializable]
        public class Settings {
            public int mapSize;
            public int maxFoodDistance;
        }
    }
}