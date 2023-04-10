using DungeonCrawl.Actors.Characters;
using UnityEngine;

namespace DungeonCrawl.Core
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Singleton { get; private set; }
        private bool _closeView;
        public (float x, float y) Position
        {
            get => _position;
            set
            {
                _position = value;
                transform.position = new Vector3(value.x, value.y + 0.5f, -10);
            }
        }

        public (float x, float y) StartingPosition;
        public float Size
        {
            get => _camera.orthographicSize;
            set => _camera.orthographicSize = value;
        }

        private (float x, float y) _position;
        private Camera _camera;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }

            Singleton = this;

            _camera = GetComponent<Camera>();

        }

    }
}
