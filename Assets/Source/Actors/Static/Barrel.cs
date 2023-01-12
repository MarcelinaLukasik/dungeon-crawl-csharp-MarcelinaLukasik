using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class Barrel : Actor
    {
        public override string assetName => "TX Props";
        public override int DefaultSpriteId => 146;
        public override string DefaultName => "Barrel";
        public override int Z => -1;

        private string spriteName;

        protected override void OnAwake()
        {
            spriteName = "Barrel";
            SpriteRend = GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId, spriteName);
        }
    }
}
