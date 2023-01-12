using UnityEngine;

namespace DungeonCrawl.Actors.Static

{
    public class Wall : Actor
    {
        public override int DefaultSpriteId => 46;
        public override string DefaultName => "Wall";
        public override (float x, float y) Position
        {
            get => _position;
            set
            {
                _position = value;
                transform.position = new Vector3(value.x, value.y -0.5f, Z);
            }
        }
        private (float x, float y) _position;
    }
}
