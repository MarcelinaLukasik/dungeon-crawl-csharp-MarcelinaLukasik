using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Actors.Static.Items;
using DungeonCrawl.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DungeonCrawl.Actors.Static
{
    public class AppleSpawner : Actor
    {
        public override string assetName => "kenney_transparent";
        public override int DefaultSpriteId => 896;
        public override string DefaultName => "AppleSpawner";
        public float Update;
        public override int Z => 2;
        public override bool Detectable => false;
        private (float x, float y) _targetPosition;
        private static AppleSpawner Singleton;
        private (int start, int end) _xSpawnRange;
        private (int start, int end) _ySpawnRange;

        protected override void OnAwake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }
            Singleton = this;
            _xSpawnRange = (1, 31);
            _ySpawnRange = (-18, -12);
        }
        protected override void OnUpdate(float deltaTime)
        {
            Update += Time.deltaTime;
            if (Update > 2.5f)
            {
                Update = 0.0f;
                System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                _targetPosition.x = random.Next(_xSpawnRange.start, _xSpawnRange.end);
                _targetPosition.y = random.Next(_ySpawnRange.start, _ySpawnRange.end);
                Actor actorAtTargetPosition = ActorManager.Singleton.GetActorAt(_targetPosition);
                if (actorAtTargetPosition == null)
                {
                    ActorManager.Singleton.Spawn<Apple>(_targetPosition);
                }
            }
        }
    }
}
