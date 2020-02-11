using System;
using UnityEngine;
using Zenject;

namespace Sheeps.Level {
    public class Food : MonoBehaviour {
        Settings _settings;
        ExplosionHandler _explosionHandler;
        Tile _tile;
        MeshRenderer meshRenderer;

        [Inject]
        public void Construct(Settings settings, ExplosionHandler explosionHandler) {
            _explosionHandler = explosionHandler;
            _settings = settings;

            meshRenderer = GetComponent<MeshRenderer>();
        }

        public Tile FoodTile { 
            get {
                return _tile;
            }
            set {
                if (_tile == value) return;

                if (_tile != null) {
                    _tile.IsFoodAttached = false;
                }
                _tile = value;
                _tile.IsFoodAttached = true;
                transform.position = value.Position;
            }
        }

        public Vector3 Position => _tile.Position;

        public Color ObjColor {
            get { return meshRenderer.material.color; }
            set { meshRenderer.material.color = value;}
        }

        public void Captured() {
            _explosionHandler.Spawn(_settings.explosionDuration, Position);
        }

        public SaveData Save() {
            return new SaveData() {
                tile = _tile,
                color = new SerializableColor(ObjColor)
            };
        }

        public void Load(SaveData saveData) {
            FoodTile = saveData.tile;
            ObjColor = saveData.color.ToColor();
        }


        [Serializable]
        public class SaveData {
            public Tile tile;
            public SerializableColor color;
        }

        [Serializable]
        public class Settings {
            public float explosionDuration;
        }

        public class Factory : PlaceholderFactory<float, Food> {

        }
    }
}