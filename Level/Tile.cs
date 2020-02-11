using System;
using UnityEngine;

namespace Sheeps.Level {

    [Serializable]
    public class Tile {
        private SerializableVector3 _position;

        public Vector3 Position {
            get {
                return _position.ToVector3();
            }
            set {
                _position = new SerializableVector3(value);
            }
        }

        public Tile(Vector3 pos) {
            Position = pos;
            IsFoodAttached = false;
        }

        public float Distance(Tile other) {
            return Distance(other.Position);
        }

        public float Distance(Vector3 otherPosition) {
            return Mathf.Abs(Position[0] - otherPosition[0]) + Mathf.Abs(Position[1] - otherPosition[1]);
        }

        public bool IsFoodAttached { get; set; }   
    }
}