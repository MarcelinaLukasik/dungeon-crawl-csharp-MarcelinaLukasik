using DungeonCrawl.Actors.Characters;
using UnityEngine;

namespace DungeonCrawl.Core
{
    /// <summary>
    ///     Class used for manipulating camera's position
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        ///     CameraController singleton
        /// </summary>
        public static CameraController Singleton { get; private set; }

        private bool _closeView;

        /// <summary>
        ///     Camera's current position
        /// </summary>
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
 

        /// <summary>
        ///     Camera's size (how much space can it see)
        /// </summary>
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

        private void Update()
        {

        }
    }
}
