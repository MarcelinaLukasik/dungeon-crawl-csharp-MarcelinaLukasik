using Assets.Source.Core;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class Slime : Character
    {
        public bool FoundPlayer;
        public override int DefaultSpriteId => 66;
        public override string DefaultName => "Slime";
        protected override void OnAwake()
        {
            SpriteRend = GetComponent<SpriteRenderer>();
            if (SpriteRend == null)
                SpriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId);
            SetHealth(15);
            Strength = 2;
        }

        protected override void OnDeath()
        {
            Debug.Log("Grrrrr");
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            Update += Time.deltaTime;
            if (Update > 1.0f)
            {
                Update = 0.0f;
                MonsterTryMove();
            }
        }
        public override void MonsterTryMove()
        {
            if (!this.FoundPlayer)
            {
                PrepareMove();
                (float x, float y) lastSlimePosition = (Position.x, Position.y);

                if (ActorAtTargetPosition == null)
                {
                    Position = TargetPosition;
                    ActorManager.Singleton.Spawn<SlimePuddle>(lastSlimePosition);
                }
                else if (ActorAtTargetPosition.GetType() == typeof(Player))
                {
                    LastPlayerPosition = ((Character)ActorAtTargetPosition).Position;
                    this.FoundPlayer = true;
                }
                else
                {
                    if (!ActorAtTargetPosition.OnCollision(this))
                    {
                        Position = TargetPosition;
                    }
                }
            }
            else
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (ActorManager.Singleton.GetActorAt(LastPlayerPosition) != null &&
                ActorManager.Singleton.GetActorAt(LastPlayerPosition).GetType() == typeof(Player))
            {
                AttackTime += Time.deltaTime;
                if (AttackTime > 0.005f)
                {
                    AttackTime = 0.0f;
                    ((Character)ActorAtTargetPosition).ApplyDamage(Strength);
                    GetComponent<Animator>().Play("attack");
                    UserInterface.Singleton.HandleTextDisplay($"Player lost {Strength} health.", UserInterface.TextPosition.TopCenter);
                }
            }
            else
            {
                FoundPlayer = false;
                GetComponent<Animator>().Play("idle");
            }
        }
    }
}
