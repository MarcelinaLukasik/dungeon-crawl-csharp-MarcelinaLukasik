using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Source.Core;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class SlimePuddle : Actor
    {
        public int Damage = 1;
        private float _lifetime = 8.0f;
        private float _update;
        public override bool OnCollision(Actor anotherActor)
        {
            return false;
        }
        public override int DefaultSpriteId => 185;
        public override string DefaultName => "Slime puddle";

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
        public void DoDamage()
        {
            Player.Singleton.ApplyDamage(Damage);
            UserInterface.Singleton.HandleTextDisplay($"Player lost {Damage} health.", UserInterface.TextPosition.TopCenter);
        }
    }
}
