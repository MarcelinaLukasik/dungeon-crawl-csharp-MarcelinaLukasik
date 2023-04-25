using System.Collections;
using UnityEngine;

namespace DungeonCrawl.Actors.Static
{
    public sealed class SecretFloor : Floor
    {
        public override string assetName => "TX Tileset Grass";
        public override int DefaultSpriteId => 2;
        public override string DefaultName => "SecretFloor";
        public override bool Detectable => false;
        public override int Z => 1;
        public static bool IsRevealable;
        private bool _isRevealed;

        private string spriteName;
        private Color _color;
        

        protected override void OnAwake()
        {
            spriteName = "Pavement 0";
            SpriteRend = GetComponent<SpriteRenderer>();
            SetSprite("TX Tileset Grass", DefaultSpriteId, spriteName);
            _color = GetComponent<Renderer>().material.color;
            _color.a = 0;
            GetComponent<Renderer>().material.color = _color;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (IsRevealable && !_isRevealed)
            {
                _isRevealed = true;
                StartCoroutine(Reveal());
            }
        }


        private IEnumerator Reveal()
        {
            for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
            {
                _color.a = alpha;
                GetComponent<Renderer>().material.color = _color;
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
