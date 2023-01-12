using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Source.Actors.Static.Items;
using Assets.Source.Core;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Core;
using UnityEngine;


namespace DungeonCrawl.Actors.Static.Items
{
    public class Crown : Item
    {
        public override string assetName => "kenney_transparent";
        public override int DefaultSpriteId => 138;
        public override string DefaultName => "Crown";
        public static bool IsRevealable;
        private bool _isRevealed;
        private Color _color;

        public static void EndGame()
        {
            UserInterface.Singleton.HandleTextDisplay("THE END!",
                UserInterface.TextPosition.MiddleCenter);
            ActorManager.Singleton.DestroyActor(Player.Singleton);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (IsRevealable && !_isRevealed)
            {
                _isRevealed = true;
                StartCoroutine(Reveal());
            }
        }

        private IEnumerator Reveal()
        {
            _color = Color.yellow;
            for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
            {
                _color.a = alpha;
                GetComponent<Renderer>().material.color = _color;
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
