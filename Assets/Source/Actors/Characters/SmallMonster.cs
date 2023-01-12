using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class SmallMonster : Character
    {
        protected override void OnDeath()
        {
            Debug.Log("That is so unfair!...");
        }

        public override int DefaultSpriteId => 63;
        public override string DefaultName => "SmallMonster";

        protected override void OnUpdate(float deltaTime)
        {
            Update += Time.deltaTime;
            if (Update > 0.3f)
            {
                Update = 0.0f;
                MonsterTryMove();
            }
        }
    }
}
