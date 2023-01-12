using DungeonCrawl.Core;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using AssemblyCSharp.Assets.Source.Actors.Static.Items;
using Assets.Source.Core;
using DungeonCrawl.Actors.Characters;
using DungeonCrawl.Actors.Static;
using DungeonCrawl.Actors.Static.Items;

namespace DungeonCrawl.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        
        public virtual (float x, float y) Position
        {
            get => _position;
            set
            {
                _position = value;
                transform.position = new Vector3(value.x, value.y, Z);
            }
        }

        public virtual (float x, float y, float z) Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                transform.Rotate(value.x, value.y, value.z);
            }
        }

        public SpriteRenderer SpriteRend;
        private (float x, float y) _position;
        private (float x, float y, float z) _rotation;
        public (float x, float y) VectorValues;
        public bool Transformed;
        public (float x, float y) LastPlayerPosition;
        public float AttackTime;
        private (float x, float y) _targetPosition;
        private Actor _actorAtTargetPosition;
        public static int MapId = 1;

        private void Start()
        {
            
        }

        private void Awake()
        {
            OnAwake();
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
            UserInterface.Singleton.SetText($"Health: {Player.Singleton.Health}", UserInterface.TextPosition.BottomLeft);
        }

        protected virtual void OnAwake()
        {
            SpriteRend = GetComponent<SpriteRenderer>();
            if (SpriteRend == null)
            {
                SpriteRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            SetSprite(assetName, DefaultSpriteId);
        }

        public void SetSprite(string assetName, int id, string spriteName = "default")
        {
            SpriteRend.sprite = ActorManager.Singleton.GetSprite(assetName, id, spriteName);
        }

        public void TryMove(Direction direction)
        {
            VectorValues = direction.ToVector();
            _targetPosition = (Position.x + VectorValues.x, Position.y + VectorValues.y);
            _actorAtTargetPosition = ActorManager.Singleton.GetActorAt(_targetPosition);

            if (_actorAtTargetPosition == null)
                Position = _targetPosition;

            else if (_actorAtTargetPosition.GetType() == typeof(LevelExit))
            {
                ActorManager.Singleton.DestroyAllActors();
                Player.Singleton.OpenDoors.Clear();
                MapId += 1;
                MapLoader.LoadMap(MapId);
               
            }
            else if (_actorAtTargetPosition.GetType() == typeof(Barrel))
            {
                ActorManager.Singleton.DestroyActor(_actorAtTargetPosition);
                System.Random rand = new System.Random();
                bool drop = rand.Next(2) == 1;
                if (drop)
                    ActorManager.Singleton.Spawn<Heart>(_targetPosition);

            }
            else if (_actorAtTargetPosition.GetType() == typeof(SmallMonster) && Transformed)
                TransformInteraction();

            else if (_actorAtTargetPosition.GetType() == typeof(Dragon))
            {
                if (Player.Singleton.CheckInventory("Sword"))
                {
                    ((Character)_actorAtTargetPosition).ApplyDamage(Player.Singleton.Strength);
                    UserInterface.Singleton.HandleTextDisplay($"Monster lost {Player.Singleton.Strength} health.", UserInterface.TextPosition.TopCenter);
                }
                    
            }
            else if (_actorAtTargetPosition.GetType() == typeof(SlimePuddle))
            {
                Position = _targetPosition;
                ((SlimePuddle)_actorAtTargetPosition).DoDamage();
            }
            else if (_actorAtTargetPosition.GetType() == typeof(Owl))
                ((Owl)_actorAtTargetPosition).Interact();
            else if (_actorAtTargetPosition.GetType() == typeof(HungryMonster) && ((HungryMonster)_actorAtTargetPosition).Detectable)
                StartCoroutine(((HungryMonster)_actorAtTargetPosition).Eat());

            else
            {
                if (!_actorAtTargetPosition.OnCollision(this))
                    Position = _targetPosition;
                else
                    InteractWithObject();
            }
        }

        private void InteractWithObject()
        {
            if (_actorAtTargetPosition.GetType() == typeof(Door))
                (_actorAtTargetPosition as Door).ManageDoors(_actorAtTargetPosition, _targetPosition, "Key");

            else if (_actorAtTargetPosition.GetType() == typeof(MagicalDoor))
                (_actorAtTargetPosition as MagicalDoor).ManageDoors(_actorAtTargetPosition, _targetPosition, "Blue Key");

            else if (_actorAtTargetPosition.GetType() == typeof(TransformObject))
                HandleTransform();
        }

        private async void HandleTransform()
        {
            this.SetSprite(assetName, 20);
            this.Transformed = true;
            await Task.Delay(10000);
            this.SetSprite(assetName, 16);
            this.Transformed = false;
        }

        private void TransformInteraction()
        {
            ActorManager.Singleton.DestroyActor(_actorAtTargetPosition);
            Player.Singleton.SmallMonsterKillCounter++;

            //TODO change magic number
            if (Player.Singleton.SmallMonsterKillCounter == 3)
                ActorManager.Singleton.Spawn<Orb>(_targetPosition);
        }

        public void RemoveItemFromInventory(string itemName)
        {
            var items = Player.Singleton.Items;
            var searchedItems = items.Where(item => item.DefaultName == itemName);
            Debug.Log(searchedItems.First().DefaultName);
            var item = searchedItems.First();
            Player.Singleton.Items.Remove(item);
            foreach (var i in Player.Singleton.Items)
            {
                Debug.Log(i);
            }
            
        }


        /// <summary>
        ///     Invoked whenever another actor attempts to walk on the same position
        ///     this actor is placed.
        /// </summary>
        /// <param name="anotherActor"></param>
        /// <returns>true if actor can walk on this position, false if not</returns>
        public virtual bool OnCollision(Actor anotherActor)
        {
            return true;
        }

        /// <summary>
        ///     Invoked every animation frame, can be used for movement, character logic, etc
        /// </summary>
        /// <param name="deltaTime">Time (in seconds) since the last animation frame</param>
        protected virtual void OnUpdate(float deltaTime)
        {
            
        }
        protected virtual void Attack()
        {

        }

        /// <summary>
        ///     Can this actor be detected with ActorManager.GetActorAt()? Should be false for purely cosmetic actors
        /// </summary>
        public virtual bool Detectable  => true;

        /// <summary>
        ///     Z position of this Actor (0 by default)
        /// </summary>
        public virtual int Z => 0;

        public virtual string assetName => "characters";

        /// <summary>
        ///     Id of the default sprite of this actor type
        /// </summary>
        public abstract int DefaultSpriteId { get; }

        /// <summary>
        ///     Default name assigned to this actor type
        /// </summary>
        public abstract string DefaultName { get; }


    }
}