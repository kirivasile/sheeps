using System;
using UnityEngine;

namespace Sheeps.Level {
    [Serializable]
    public class SerializableVector3 {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 vector) {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3() {
            return new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class SerializableColor {
        public float r;
        public float g;
        public float b;

        public SerializableColor(Color color) {
            r = color.r;
            g = color.g;
            b = color.b;
        }

        public Color ToColor() {
            return new Color(r, g, b);
        }
    }
}