using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sheeps.Level {
    public class Animal : MonoBehaviour {
        Food _food;
        MeshRenderer _meshRenderer;
        float _speed;
        TileMap _map;

        Vector3 _currentDirection;

        float _timeToNextPreyDetection;
        float _preyDetectionPeriod = 1f;

        [Inject]
        public void Construct(float speed, Food food, TileMap map) {
            _food = food;
            _speed = speed;
            _map = map;

            _timeToNextPreyDetection = _preyDetectionPeriod;

            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public Vector3 Position {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Tile FoodTile {
            get { return _food.FoodTile; }
            set { _food.FoodTile = value; }
        }

        public Color ObjColor {
            get { return _meshRenderer.material.color; }
            set { 
                _meshRenderer.material.color = value; 
                _food.ObjColor = value;
            }
        }

        public void FixedTick() {
            _timeToNextPreyDetection -= Time.deltaTime;

            if (IsAnimalInTileCenter()) {
                DetectPrey();
            }
            Prey();
        }

        bool IsAnimalInTileCenter() {
            return
                Mathf.Abs(Position.x - Mathf.Round(Position.x)) < 5e-2 &&
                Mathf.Abs(Position.y - Mathf.Round(Position.y)) < 5e-2;
        }

        void DetectPrey() {
            // Custom algorithm
            // Try to move by X, then move by Y. Or vice versa, depends on Random.value > 0.5
            bool byX = (Random.value > 0.5);

            if (byX) {
                if (Mathf.Abs(Position.x - _food.Position.x) < 5e-2) {
                    Position = new Vector3(_food.Position.x, Position.y);
                    if (Position.y > _food.Position.y) {
                        _currentDirection = Vector3.down;
                    } else {
                        _currentDirection = Vector3.up;
                    }
                } else {
                    if (Position.x > _food.Position.x) {
                        _currentDirection = Vector3.left;
                    } else {
                        _currentDirection = Vector3.right;
                    }
                }
            } else {
                if (Mathf.Abs(Position.y - _food.Position.y) < 5e-2) {
                    Position = new Vector3(Position.x, _food.Position.y);
                    if (Position.x > _food.Position.x) {
                        _currentDirection = Vector3.left;
                    } else {
                        _currentDirection = Vector3.right;
                    }
                } else {
                    if (Position.y > _food.Position.y) {
                        _currentDirection = Vector3.down;
                    } else {
                        _currentDirection = Vector3.up;
                    }
                }
            }
            
        }

        void Prey() {
            Position += _speed * _currentDirection * Time.deltaTime;
        }

        void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Food>() == _food) {
                Position = _food.Position;
                _food.Captured();
                _food.FoodTile = _map.SampleFoodPosition(Position);
            }
            if (other.GetComponent<Animal>() != null) {
                List<Vector3> availableDirections = _map.GetAvailableDirections(Position);
                if (availableDirections.Contains(-_currentDirection)) {
                    _currentDirection = -_currentDirection;
                }
            }
        }

        public SaveData Save() {
            return new SaveData() {
                food = _food.Save(),
                position = new SerializableVector3(Position),
                direction = new SerializableVector3(_currentDirection),
                speed = _speed
            };
        }

        public void Load(SaveData saveData) {
            _food.Load(saveData.food);
            Position = saveData.position.ToVector3();
            _currentDirection = saveData.direction.ToVector3();
            _speed = saveData.speed;
            ObjColor = saveData.food.color.ToColor();
        }

        [Serializable]
        public class SaveData {
            public Food.SaveData food;
            public SerializableVector3 position;
            public SerializableVector3 direction;
            public float speed;
        }

        public class Factory : PlaceholderFactory<float, Animal> {

        }
    }
}