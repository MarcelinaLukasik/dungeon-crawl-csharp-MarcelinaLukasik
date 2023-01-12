using AssemblyCSharp.Assets.Source.Actors.Static.Items;
using DungeonCrawl.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawl.Actors.Static.Items;
using Assets.Source.Core;
using DungeonCrawl.Actors.Static;


namespace DungeonCrawl.Actors.Characters
{
    public class Player : Character
    {
        public static Player Singleton { get; private set; }
        public int SmallMonsterKillCounter;
        public List<Item> Items;
        public List<Door> OpenDoors;
        public List<Item> PickedUpItems;
        private bool _closeView;
        private bool _toggledMenu;
        private float _lastStep, timeBetweenSteps = 0.15f;
        private Data data = new Data();

        protected override void OnAwake()
        {
            Singleton = this;
            SpriteRend = GetComponent<SpriteRenderer>();
            if (SpriteRend == null)
                SpriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId);
            SetHealth(50);
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            if (Input.GetKey(KeyCode.W))
                Step(Direction.Up);

            if (Input.GetKey(KeyCode.S))
                Step(Direction.Down);

            if (Input.GetKey(KeyCode.A))
                Step(Direction.Left);

            if (Input.GetKey(KeyCode.D))
                Step(Direction.Right);

            if (Input.GetKeyDown(KeyCode.E))
                PickUpItem();

            if (Input.GetKeyDown(KeyCode.I))
                DisplayInventory();

            if (Input.GetKeyDown(KeyCode.V))
                _closeView = !_closeView;
            if (Input.GetKeyDown(KeyCode.F5))
            {
                data.HandleSave(MapId);
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                data.LoadGame();
                UserInterface.Singleton.HandleTextDisplay($"Game loaded", UserInterface.TextPosition.TopCenter);
            }
            
            ChangeView();
        }

        private void ChangeView()
        {
            if (_closeView)
            {
                CameraController.Singleton.Size = 5;
                CameraController.Singleton.Position = Position;
            }
            else
            {
                CameraController.Singleton.Size = 10;
                CameraController.Singleton.Position = CameraController.Singleton.StartingPosition;
            }
        }

        private void Step(Direction direction)
        {
            if (Time.time - _lastStep > timeBetweenSteps)
            {
                _lastStep = Time.time;
                TryMove(direction);
            }
        }

        private void PickUpItem()
        {
            if (ActorManager.Singleton.GetActorAt<Item>(this.Position) != null)
            {
                
                Item item = ActorManager.Singleton.GetActorAt<Item>(this.Position);
                ActorManager.Singleton.DestroyActor(item);
                UserInterface.Singleton.HandleTextDisplay($"Picked up {item.DefaultName}", UserInterface.TextPosition.BottomCenter);
                if (item.GetType() == typeof(Crown))
                    Crown.EndGame();
                if (item.GetType() == typeof(Heart))
                {
                    Health += 5;
                    return;
                }
                Items.Add(item);
                PickedUpItems.Add(item);
                if (item.GetType() == typeof(Sword))
                    Strength += 5;
            }
            else
                UserInterface.Singleton.HandleTextDisplay("Nothing to pick up", UserInterface.TextPosition.BottomCenter);
        }

        public void InitializeLists()
        {
            Items = new List<Item>();
            OpenDoors = new List<Door>();
            PickedUpItems = new List<Item>();
        }

        public bool CheckInventory(string itemName)
        {
            var item = Items.Where(item => item.DefaultName == itemName);
            return item.Any();
        }

        public int CountItemsInInventory(string itemName)
        {
            return Items.Count(item => item.DefaultName == itemName);
        }

        public void DisplayInventory()
        {
            if (!_toggledMenu)
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("Items:\n");
                foreach (var item in Items)
                    sb.AppendLine($"{item.DefaultName}\n");

                UserInterface.Singleton.SetText(sb.ToString(), UserInterface.TextPosition.TopLeft);
                _toggledMenu = true;
            }
            else
            {
                UserInterface.Singleton.RemoveText(UserInterface.TextPosition.TopLeft);
                _toggledMenu = false;
            }
        }


        public override bool OnCollision(Actor anotherActor)
        {
            return true;
        }

        protected override void OnDeath()
        {
            UserInterface.Singleton.SetText("GAME OVER", UserInterface.TextPosition.MiddleCenter);
        }

        public override int DefaultSpriteId => 16;
        public override string DefaultName => "Player";
    }
}
