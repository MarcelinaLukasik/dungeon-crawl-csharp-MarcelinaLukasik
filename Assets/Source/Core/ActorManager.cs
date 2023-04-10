using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawl.Actors;
using DungeonCrawl.Actors.Characters;
using UnityEngine;
using UnityEngine.U2D;
using Random = System.Random;

namespace DungeonCrawl.Core
{

    public class ActorManager : MonoBehaviour
    {
        public static ActorManager Singleton { get; private set; }

        private SpriteAtlas _spriteAtlas;
        public HashSet<Actor> AllActors;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }

            Singleton = this;

            AllActors = new HashSet<Actor>();
            _spriteAtlas = Resources.Load<SpriteAtlas>("Spritesheet");
        }

        public Actor GetActorAt((float x, float y) position)
        {
            return AllActors.FirstOrDefault(actor => actor.Detectable && actor.Position == position);
        }
        public Actor GetLastActorAt((float x, float y) position)
        {
            return AllActors.LastOrDefault(actor => actor.Detectable && actor.Position == position);
        }

        public T GetActorAt<T>((float x, float y) position) where T : Actor
        {
            return AllActors.FirstOrDefault(actor => actor.Detectable && actor is T && actor.Position == position) as T;
        }

        public void DestroyActor(Actor actor)
        {
            AllActors.Remove(actor);
            Destroy(actor.gameObject);
        }

        public void DestroyAllActors()
        {
            var actors = AllActors.ToArray();

            foreach (var actor in actors)
            {
                if (actor.GetType() == typeof(Player))
                    continue;
                DestroyActor(actor);
            }
                
        }

        public Sprite GetSprite(string assetName, int id, string spriteName = "default")
        {
            if (spriteName == "default")
                return _spriteAtlas.GetSprite($"{assetName}_{id}");
            return _spriteAtlas.GetSprite($"{assetName} {spriteName}");
            // return _spriteAtlas.GetSprite($"TX Props Wooden Gate");
        }

        public T Spawn<T>((float x, float y) position, string actorName = null) where T : Actor
        {
            return Spawn<T>(position.x, position.y, actorName);
        }
        public T Spawn<T>((float x, float y) position, (float x, float y, float z) rotation,
            string actorName = null) where T : Actor
        {
            return Spawn<T>(position.x, position.y, rotation, actorName);
        }

        public T Spawn<T>(float x, float y, string actorName = null) where T : Actor
        {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            var component = go.AddComponent<T>();
            go.name = actorName ?? component.DefaultName;
            component.Position = (x, y);
            AllActors.Add(component);
            return component;
        }

        public T Spawn<T>(float x, float y, (float x, float y, float z) rotation, string actorName = null) where T : Actor
        {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            var component = go.AddComponent<T>();
            go.name = actorName ?? component.DefaultName;
            component.Position = (x, y);
            component.Rotation = (rotation.x, rotation.y, rotation.z);
            AllActors.Add(component);
            return component;
        }

        public T SpawnPrefab<T>((float x, float y) position, string actorName = null) where T : Actor
        {
            var go = Instantiate(Resources.Load<GameObject>("Character (21)"));
            go.GetComponent<Animator>().Play("idle");
            var component = go.AddComponent<T>();
            go.name = actorName ?? component.DefaultName;
            component.Position = (position.x, position.y);
            AllActors.Add(component);
            return component;
        }

        public int GetRandomSprite()
        {
            var spritesId = new List<int> { 146, 147, 150, 151, 153, 154, 155, 217, 219 };
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int randomId = random.Next(spritesId.Count);
            return spritesId[randomId];
        }
    }
}
