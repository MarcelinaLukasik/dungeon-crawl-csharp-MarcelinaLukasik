using UnityEngine;


namespace DungeonCrawl.Actors.Static
{
    public class Plant : Actor
    {
        public override string assetName => "TX Bush";
        public override int DefaultSpriteId => 2;
        public override string DefaultName => "Plant";
        public override int Z => -1;

        private string _spriteName;

        protected override void OnAwake()
        {
            _spriteName = "T4";
            SpriteRend = GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId, _spriteName);
        }
    }
}
