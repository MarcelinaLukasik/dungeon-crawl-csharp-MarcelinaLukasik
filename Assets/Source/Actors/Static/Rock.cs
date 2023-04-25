using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class Rock : Actor
    {
        public override int DefaultSpriteId => 154;
        public override string DefaultName => "Rock";


        protected override void OnAwake()
        {
            int randomId = ActorManager.Singleton.GetRandomSprite();
            SpriteRend = GetComponent<SpriteRenderer>();
            SetSprite(assetName, randomId);
        }
    }
}
