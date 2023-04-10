using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Actors.Static.Items;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DungeonCrawl.Core
{
    public static class MapLoader
    {
        public static void LoadMap(int id)
        {
            var lines = Regex.Split(Resources.Load<TextAsset>($"map_{id}").text, "\r\n|\r|\n");
            var split = lines[0].Split(' ');
            var width = int.Parse(split[0]);
            var height = int.Parse(split[1]);
            float _horizontalRotation = 90.0f;
           
            for (var y = 0; y < height; y++)
            {
                var line = lines[y + 1];
                for (var x = 0; x < width; x++)
                {
                    var character = line[x];
                    SpawnActor(character, (x, -y), (0.0f, 0.0f, _horizontalRotation));
                }
            }
           
            CameraController.Singleton.Size = 10;
            CameraController.Singleton.Position = (width / 2, -height / 2);
            CameraController.Singleton.StartingPosition = (width / 2, -height / 2);
        }


        private static void SpawnActor(char c, (float x, float y) position, (float x, float y, float z) rotation)
        {
            switch (c)
            {
                case '#':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Wall>(position);
                    break;
                case '.':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case ',':
                    ActorManager.Singleton.Spawn<SecretFloor>(position);
                    break;
                case 'k':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Key>(position);
                    break;
                case 'K':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<MagicalKey>(position);
                    break;
                case 'S':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Sword>(position);
                    break;
                case 's':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<Slime>(position);
                    break;
                case 'p':
                    if (Player.Singleton == null)
                    {
                        ActorManager.Singleton.SpawnPrefab<Player>(position);
                    }
                    ActorManager.Singleton.Spawn<Floor>(position);
                    break;
                case 'g':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<Dragon>(position);
                    break;
                case 'd':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<MagicalDoor>(position);
                    break;
                case 'D':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Door>(position, rotation);
                    break;
                case 'e':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<LevelExit>(position);
                    break;
                case 't':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<TransformObject>(position);
                    break;
                case 'm':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<SmallMonster>(position);
                    break;
                case 'o':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<Owl>(position);
                    break;
                case 'r':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Rock>(position, "Rock");
                    break;
                case 'b':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Barrel>(position);
                    break;
                case 'n':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<Plant>(position);
                    break;
                case 'H':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.SpawnPrefab<HungryMonster>(position);
                    break;
                case 'a':
                    ActorManager.Singleton.Spawn<Floor>(position);
                    ActorManager.Singleton.Spawn<AppleSpawner>(position);
                    break;
                case ' ':
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
