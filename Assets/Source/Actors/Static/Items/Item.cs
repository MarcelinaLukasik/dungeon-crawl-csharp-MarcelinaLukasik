using System;
using DungeonCrawl.Actors;
namespace AssemblyCSharp.Assets.Source.Actors.Static.Items
{
    public abstract class Item : Actor
    {
        public override bool OnCollision(Actor anotherActor)
        {
            return false;
        }

    }
}

