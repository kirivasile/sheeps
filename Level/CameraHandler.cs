using System;
using UnityEngine;
using Zenject;

namespace Sheeps.Level {
    public class CameraHandler : ITickable {
        Settings _settings;
        Camera _camera;

        const float TILE_HALF = 0.5f;
        float minZoom = TILE_HALF;
        float maxZoom = TILE_HALF;
        int _mapSize;

        Vector3 Position {
            get { return _camera.transform.position; }
            set { _camera.transform.position = value; }
        }

        public CameraHandler(
            [Inject] Settings settings, 
            [Inject(Id = "Main")] Camera camera
        ) {
            _settings = settings;
            _camera = camera;
        }

        public void ChangeMapSize(int mapSize) {
            _mapSize = mapSize;
            maxZoom = (float)mapSize / 2f;

            _camera.orthographicSize = maxZoom;
            Position = new Vector3(
                maxZoom - TILE_HALF,
                maxZoom - TILE_HALF,
                -10f
            );
        }

        public void Tick() {
            HandleMouseZoom();
            HandleMouseMovement();
        }

        void HandleMouseZoom() {
            float scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scrollWheelChange) > 1e-4) {
                float curZoom = _camera.orthographicSize;
                curZoom += scrollWheelChange * _settings.zoomSensitivity;
                curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
                _camera.orthographicSize = curZoom;
                Position = ClampCameraPosition(Position);
            }
        }

        void HandleMouseMovement() {
            Vector3 direction = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow)) {
                direction += Vector3.up;
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                direction += Vector3.right;
            }

            if (direction.magnitude > 1e-4) {
                Vector3 newPosition = Position + direction * _settings.speed;
                Position = ClampCameraPosition(newPosition);
            }
        }

        Vector3 ClampCameraPosition(Vector3 newPosition) {
            float curZoom = _camera.orthographicSize;
            float leftBorder = curZoom - TILE_HALF;
            float rightBorder = (maxZoom - TILE_HALF)* 2f - leftBorder;

            newPosition.x = Mathf.Clamp(newPosition.x, leftBorder, rightBorder);
            newPosition.y = Mathf.Clamp(newPosition.y, leftBorder, rightBorder);

            return newPosition;
        }

        [Serializable]
        public class Settings {
            public float zoomSensitivity;
            public float speed;
        }
    }
}