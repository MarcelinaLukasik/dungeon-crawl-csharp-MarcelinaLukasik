using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Source.Core;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static.Items;
using DungeonCrawl.Core;

namespace DungeonCrawl.Actors.Static
{
    public class Owl : Actor
    {
        public override int DefaultSpriteId => 178;
        public override string DefaultName => "Owl";

        public void Interact()
        {
            var orb = Player.Singleton.CheckInventory("Orb");
            if (!orb)
                UserInterface.Singleton.HandleTextDisplay("Those little monsters stole my magic essence!",
                    UserInterface.TextPosition.TopRight);
            else
            {
                UserInterface.Singleton.HandleTextDisplay("Thank you, here is your reward!",
                    UserInterface.TextPosition.TopRight);
                ActorManager.Singleton.Spawn<MagicalKey>(Position.x - 1, Position.y);
            }
        }
    }
}
