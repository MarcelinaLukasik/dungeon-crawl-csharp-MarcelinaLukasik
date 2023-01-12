using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Source.Core;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Actors.Static.Items;
using DungeonCrawl.Core;
using UnityEngine;

namespace DungeonCrawl.Actors.Characters
{
    public class HungryMonster : Character
    {
        public override int DefaultSpriteId => 35;
        public override string DefaultName => "HungryMonster";
        private int Fed = 3;
        private int _eatenApples;
        private List<Tuple<float, float>> _wallsToDestroy;
        private bool _destroyed;
        private float _speed;
        private int _frameRate;
        private (float x, float y) _crownPosition;
        public new bool Detectable { get; private set; }

        protected override void OnAwake()
        {
            SpriteRend = GetComponent<SpriteRenderer>();
            if (SpriteRend == null)
                SpriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
            SetSprite(assetName, DefaultSpriteId);
            _wallsToDestroy = new List<Tuple<float, float>>();
            _wallsToDestroy.Add(new Tuple<float, float>(19, -11));
            _wallsToDestroy.Add(new Tuple<float, float>(19, -10));
            _wallsToDestroy.Add(new Tuple<float, float>(19, -9));
            Detectable = true;
            _frameRate = 100;
            _crownPosition = (4, -10);
        }

        protected override void OnDeath()
        {
            Debug.Log("So hungry...");
        }

        public IEnumerator Eat()
        {
            int appleCount = Player.Singleton.CountItemsInInventory("Apple");
            Color c = transform.GetChild(0).GetComponent<Renderer>().material.color;
            Vector3 scaleChange = new Vector3(+0.01f, +0.01f, +0.01f);

            for (int i = 0; i < appleCount; i++)
            {
                Player.Singleton.RemoveItemFromInventory("Apple");
                _eatenApples += 1;
                for (int animationFrames = 0; animationFrames < _frameRate; animationFrames++)
                {
                    c += new Color((1.0f / Fed) / _frameRate, -0.2f / _frameRate, -0.2f / _frameRate, 0.0f);
                    transform.GetChild(0).GetComponent<Renderer>().material.color = c;
                    transform.localScale += scaleChange;
                    yield return new WaitForSeconds(.01f);
                }

            }
            if (_eatenApples >= Fed)
            {
                Detectable = false;
                StartCoroutine(Run());
            }
            else
            {
                UserInterface.Singleton.HandleTextDisplay($"Still hungry...", UserInterface.TextPosition.BottomRight);
            }
        }

        private IEnumerator Fade()
        {
            Color c = transform.GetChild(0).GetComponent<Renderer>().material.color;
            for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
            {
                c.a = alpha;
                transform.GetChild(0).GetComponent<Renderer>().material.color = c;
                yield return new WaitForSeconds(.05f);
            }
            ActorManager.Singleton.DestroyActor(this);
        }
        IEnumerator Run()
        {
            for (int animationFrames = 0; animationFrames < 50; animationFrames ++)
            {
                _speed += 2.0f;
                transform.position += new Vector3(-1  * _speed * Time.deltaTime , 0, 0);
                if (transform.position.x < 21 && !_destroyed)
                {
                    foreach (var wallTuple in _wallsToDestroy)
                    {
                        Actor actor = ActorManager.Singleton.GetActorAt((wallTuple.Item1, wallTuple.Item2));
                        ActorManager.Singleton.DestroyActor(actor);
                    }
                    _destroyed = true;
                }
                yield return new WaitForSeconds(.05f);
            }
            Position = ((int)transform.position.x, (int)transform.position.y);
            SecretFloor.IsRevealable = true;
            Crown.IsRevealable = true;
            StartCoroutine(Fade());
            ActorManager.Singleton.Spawn<Crown>(4, -10);
        }

        public override bool OnCollision(Actor anotherActor)
        {
            if (Detectable)
                return true;
            else
                return false;
        }
    }
}
