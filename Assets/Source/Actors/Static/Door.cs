using DungeonCrawl.Actors.Static.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Actors.Characters;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public class Door : Actor
    {
        public bool Open;
        public override string assetName => "kenney_transparent";
        public override int DefaultSpriteId => 146;
        public override string DefaultName => "Door";


        public override bool OnCollision(Actor anotherActor)
        {
            if (Open)
            {
                return false;
            }
            return true;
        }

        public void ManageDoors(Actor actorAtTargetPosition, (float x, float y) targetPosition, string keyName)
        {
            bool key = Player.Singleton.CheckInventory(keyName);
            if (key)
            {
                HandleDoorsOpening(actorAtTargetPosition, keyName);
            }
        }

        private void HandleDoorsOpening(Actor actorAtTargetPosition, string keyName)
        {
            if (gameObject.name == "MagicalDoor")
            {
                actorAtTargetPosition.SetSprite("kenney_transparent", 433);
            }
            else
                actorAtTargetPosition.SetSprite("kenney_transparent", 147);
            (actorAtTargetPosition as Door).Open = true;
            RemoveItemFromInventory(keyName);
            Player.Singleton.OpenDoors.Add((Door)actorAtTargetPosition);
        }
    }
}
