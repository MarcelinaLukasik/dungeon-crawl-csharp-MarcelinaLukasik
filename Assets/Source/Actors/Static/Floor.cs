using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class Floor : Actor
    {
        public override string assetName => "TX Tileset Stone";
        public override int DefaultSpriteId => 2;
        public override string DefaultName => "Floor";
        public override bool Detectable => false;
        public override int Z => 1;

        private string _spriteName;

        protected override void OnAwake()
        {
            if (MapId == 1)
            {
                
                _spriteName = "Ground_9";
                SpriteRend = GetComponent<SpriteRenderer>();
                SetSprite(assetName, DefaultSpriteId, _spriteName);
            }
            else
            {
                _spriteName = "9";
                SpriteRend = GetComponent<SpriteRenderer>();
                SetSprite("TX Tileset Grass", DefaultSpriteId, _spriteName);
            }
        }
    }
}
