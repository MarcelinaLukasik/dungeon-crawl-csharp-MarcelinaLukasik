using DungeonCrawl.Actors.Characters;
using UnityEngine;

namespace DungeonCrawl.Core
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            MapLoader.LoadMap(1);
            Player.Singleton.InitializeLists();
        }
    }
}
