using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Source.Actors.Static.Items;
using DungeonCrawl.Core;
using UnityEngine;


namespace DungeonCrawl.Actors.Static.Items
{
    public class Apple : Item
    {
        public override string assetName => "kenney_transparent";
        public override int DefaultSpriteId => 896;
        public override string DefaultName => "Apple";

        private float _lifetime = 3.0f;

        private float _update;

        protected override void OnUpdate(float deltaTime)
        {
            
            if (_update < _lifetime)
            {
                _update += Time.deltaTime;
            }
            else
            {
                ActorManager.Singleton.DestroyActor(this);
            }
        }
    }

}
