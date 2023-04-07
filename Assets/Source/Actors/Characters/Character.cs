using System;
using System.Collections.Generic;
using System.Linq;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public abstract class Character : Actor
    {
        public int Health;
        public float Update;
        public int Strength;
        public (float x, float y) TargetPosition;
        public Actor ActorAtTargetPosition;

        public void ApplyDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                OnDeath();
                ActorManager.Singleton.DestroyActor(this);
            }
        }
        public void SetHealth(int health)
        {
            Health = health;
        }

        public void PrepareMove()
        {
            System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
            Type type = typeof(Direction);
            Array directions = type.GetEnumValues();
            int index = random.Next(directions.Length);
            Direction direction = (Direction)directions.GetValue(index);
            VectorValues = direction.ToVector();
            TargetPosition = (Position.x, Position.y);
            TargetPosition = (TargetPosition.x + VectorValues.x, TargetPosition.y + VectorValues.y);
            ActorAtTargetPosition = ActorManager.Singleton.GetActorAt(TargetPosition);
        }

        public virtual void MonsterTryMove()
        {
            PrepareMove();

            if (ActorAtTargetPosition == null)
            {
                Position = TargetPosition;
            }
            else
            {
                if (!ActorAtTargetPosition.OnCollision(this))
                {
                    Position = TargetPosition;
                }
            }
        }

        protected abstract void OnDeath();
        public override int Z => -1;
    }
}
