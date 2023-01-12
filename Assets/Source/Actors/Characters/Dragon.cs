using System.Collections.Generic;
using Assets.Source.Core;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class Dragon : Character
    {
        public bool FoundPlayer;
        
        protected override void OnAwake()
        {
            SpriteRend = GetComponent<SpriteRenderer>();
            if (SpriteRend == null)
                SpriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId);
            SetHealth(15);
            Strength = 3;
        }
        
        protected override void OnDeath()
        {
            Debug.Log("I was dead inside anyways...");
        }
        public override int DefaultSpriteId => 11;
        public override string DefaultName => "Goblin";
        protected override void OnUpdate(float deltaTime)
        {
            Update += Time.deltaTime;
            if (Update > 2.0f)
            {
                Update = 0.0f;
                MonsterTryMove();
            }
        }
        public override void MonsterTryMove()
        {
            CheckSurroundings();

            if (!this.FoundPlayer)
            {
                PrepareMove();
                
                if (ActorAtTargetPosition == null)
                {
                    // No obstacle found, just move
                    Position = TargetPosition;
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
                        // Allowed to move
                        Position = TargetPosition;
                    }
                }
            } 
            else
                Attack();
        }
        protected override void Attack()
        {
            if (ActorManager.Singleton.GetActorAt(LastPlayerPosition) != null &&
                ActorManager.Singleton.GetActorAt(LastPlayerPosition).GetType() == typeof(Player))
            {
                AttackTime += Time.deltaTime;
                if (AttackTime > 0.003f)
                {
                    AttackTime = 0.0f;
                    ((Character)ActorManager.Singleton.GetActorAt(LastPlayerPosition)).ApplyDamage(Strength);
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

        private void CheckSurroundings()
        {
            List< (float x, float y)> vecList = new List<(float x, float y)>()
            {
                (0,1),
                (0,-1),
                (-1,0),
                (1, 0)
            };

            foreach (var vector in vecList)
            {
                if (ActorManager.Singleton.GetActorAt((Position.x + vector.x, Position.y + vector.y)) != null &&
                    ActorManager.Singleton.GetActorAt((Position.x + vector.x, Position.y + vector.y)).GetType() ==
                    typeof(Player))
                {
                    LastPlayerPosition = (Position.x + vector.x, Position.y + vector.y);
                    this.FoundPlayer = true;
                }
            }
        }
    }
}
