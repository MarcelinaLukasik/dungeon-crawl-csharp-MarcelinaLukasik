using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Source.Actors.Static.Items;

namespace DungeonCrawl.Actors.Static.Items
{
    public class Orb : Item
    {
        public override string assetName => "kenney_transparent";
        public override int DefaultSpriteId => 618;
        public override string DefaultName => "Orb";
    }
}
