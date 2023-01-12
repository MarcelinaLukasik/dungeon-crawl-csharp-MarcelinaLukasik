using System.Collections.Generic;
using System.IO;
using AssemblyCSharp.Assets.Source.Actors.Static.Items;
using Assets.Source.Core;
using DungeonCrawl.Actors;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Actors.Static.Items;
using UnityEngine;

namespace DungeonCrawl.Core
{
    public class Data
    {
        private List<string> PlayerData = new List<string>();
        public string ItemsNames;
        private string _doors;
        private string _itemsPosition;

        public void HandleSave(int mapId)
        {
            GetPlayerData(mapId);
            SaveGame();
            UserInterface.Singleton.HandleTextDisplay($"Game saved", UserInterface.TextPosition.TopCenter);
        }
        private void GetPlayerData(int mapId)
        {
            ClearData();
            PlayerData.Add(Player.Singleton.Health.ToString());
            PlayerData.Add($"{Player.Singleton.Position.x}; {Player.Singleton.Position.y}");
            PlayerData.Add(mapId.ToString());
            foreach (var item in Player.Singleton.Items)
            {
                ItemsNames += $"{item.DefaultName};";
            }
            if (ItemsNames != "")
                ItemsNames = ItemsNames.Remove(ItemsNames.Length - 1);

            PlayerData.Add(ItemsNames);

            _itemsPosition = AddPositions(Player.Singleton.PickedUpItems, _itemsPosition);
            PlayerData.Add(_itemsPosition);

            _doors = AddPositions(Player.Singleton.OpenDoors, _doors);
            PlayerData.Add(_doors);

        }
        private void SaveGame()
        {
            var writer = new StreamWriter("SaveGameFile.csv");
            for (int i = 0; i < PlayerData.Count; ++i)
            {
                if (PlayerData.Count == i + 1)
                    writer.Write($"{PlayerData[i]}");
                else
                    writer.Write($"{PlayerData[i]},");
            }
            writer.Flush();
        }

        public void LoadGame()
        {
            Player.Singleton.Items.Clear();

            var reader = new StreamReader("SaveGameFile.csv");
            var playerData = reader.ReadToEnd();
            var stats = playerData.Split('\u002C');

            Player.Singleton.Health = int.Parse(stats[0]);
            var position = stats[1].Split(';');
            Actor.MapId = int.Parse(stats[2]);
            var items = stats[3].Split(';');
            if (items[0] != "")
                foreach (var item in items)
                    HandleAddToInventory(item);

            ActorManager.Singleton.DestroyAllActors();
            MapLoader.LoadMap(Actor.MapId);
            var itemsPosition = stats[4].Split(';');
            DestroyAlreadyPickedUpItems(itemsPosition);

            var doorsPosition = stats[5].Split(';');
            OpenDoorsAfterLoad(doorsPosition);

            Player.Singleton.Position = (float.Parse(position[0]), float.Parse(position[1]));
        }

        public void ClearData()
        {
            PlayerData.Clear();
            ItemsNames = "";
            _doors = "";
            _itemsPosition = "";
        }

        private void LoadInventory<T>() where T : Item
        {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            var component = go.AddComponent<T>();
            Player.Singleton.Items.Add(component);
            ActorManager.Singleton.DestroyActor(component);
        }

        private void HandleAddToInventory(string item)
        {
            switch (item)
            {
                case "Sword":
                    LoadInventory<Sword>();
                    break;
                case "Key":
                    LoadInventory<Key>();
                    break;
                case "Blue Key":
                    LoadInventory<MagicalKey>();
                    break;
                case "Apple":
                    LoadInventory<Apple>();
                    break;
            }
        }

        private void DestroyAlreadyPickedUpItems(string[] itemsPosition)
        {
            if (itemsPosition[0] != "")
                foreach (var itemPosition in itemsPosition)
                {
                    var coordinates = itemPosition.Split(' ');
                    var actor = ActorManager.Singleton.GetLastActorAt((float.Parse(coordinates[0]), float.Parse(coordinates[1])));
                    if (actor != null)
                        ActorManager.Singleton.DestroyActor(actor);
                }
        }

        private void OpenDoorsAfterLoad(string[] doorsPosition)
        {
            if (doorsPosition[0] != "")
                foreach (var doorPosition in doorsPosition)
                {
                    var coordinates = doorPosition.Split(' ');
                    var actor = ActorManager.Singleton.GetActorAt((float.Parse(coordinates[0]), float.Parse(coordinates[1])));
                    Player.Singleton.OpenDoors.Add(actor as Door);

                    if (actor.DefaultName == "MagicalDoor")
                    {
                        actor.SetSprite("kenney_transparent", 433);
                    }
                    else
                        actor.SetSprite("kenney_transparent", 147);
                    (actor as Door).Open = true;
                }
        }

        private string AddPositions(List<Item> actorList, string allPositions)
        {
            foreach (var item in actorList)
            {
                allPositions += $"{item.Position.x} {item.Position.y};";
            }
            if (allPositions != "")
                allPositions = allPositions.Remove(allPositions.Length - 1);
            return allPositions;
        }
        private string AddPositions(List<Door> actorList, string allPositions)
        {
            foreach (var item in actorList)
            {
                allPositions += $"{item.Position.x} {item.Position.y};";
            }
            if (allPositions != "")
                allPositions = allPositions.Remove(allPositions.Length - 1);
            return allPositions;
        }
    }
}
