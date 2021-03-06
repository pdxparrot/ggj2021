using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Interactables;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class Sheep : NPC3D, IInteractable
    {
        public SheepBehavior SheepBehavior => (SheepBehavior)NPCBehavior;

        public bool CanInteract => !SheepBehavior.IsCarried;

        public bool CanCatch => CanInteract && !SheepBehavior.IsLaunched;

        public Type InteractableType => typeof(Sheep);

        public bool IsInQueue => SheepBehavior.IsEnqueued;

        public bool CanScore => SheepBehavior.IsLaunched;

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnPoint;

        [SerializeField]
        [ReadOnly]
        private string _tag;

        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                _sheepModel.SkinHelper.Skin = _tag;
            }
        }

        public bool HasTag => !string.IsNullOrWhiteSpace(Tag);

        private SheepModel _sheepModel;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            Collider.isTrigger = true;

            GetComponent<AudioSource>().spatialBlend = 0.0f;

            _sheepModel = Model.GetComponent<SheepModel>();

            SetObstacle();
        }

        protected override void OnDestroy()
        {
            if(NPCManager.HasInstance) {
                NPCManager.Instance.UnregisterNPC(this);
            }

            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            // hacky fix for the sheep not wanting to stay centered
            if(SheepBehavior.IsCarried) {
                transform.localPosition = Vector3.zero;
            }
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is SheepBehavior);
        }

        private void SetCarried(Transform parent, bool carried)
        {
            transform.SetParent(parent);

            Model.gameObject.SetActive(!carried);

            Rigidbody.isKinematic = carried;

            if(carried) {
                Movement.Position = parent.position;
            }
        }

        public void ReSpawn()
        {
            Debug.Log($"Respawning sheep {Id}");
            _spawnPoint.ReSpawn(this);
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            NPCManager.Instance.RegisterNPC(this);

            _spawnPoint = spawnpoint;

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            NPCManager.Instance.RegisterNPC(this);

            return true;
        }

        public override void OnDeSpawn()
        {
            NPCManager.Instance.UnregisterNPC(this);

            base.OnDeSpawn();
        }

        #endregion

        #region Event Handlers

        public void OnCarried(Transform parent)
        {
            SetCarried(parent, true);

            SheepBehavior.OnCarried();
        }

        public void OnEnqueued(Transform target)
        {
            SheepBehavior.OnEnqueued(target);
        }

        public void OnFree()
        {
            SheepBehavior.OnFree();
        }

        public void OnLaunch(Vector3 start, Vector3 direction)
        {
            SetCarried(GameManager.Instance.BaseLevel.SheepPen, false);

            SheepBehavior.OnLaunch(start, direction);
        }

        public void OnDropped()
        {
            SetCarried(GameManager.Instance.BaseLevel.SheepPen, false);

            SheepBehavior.OnDropped();
        }

        public void OnScored()
        {
            DeSpawn(true);
        }

        public void OnPlatformEnter(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void OnPlatformExit()
        {
            transform.SetParent(GameManager.Instance.BaseLevel.SheepPen);
        }

        #endregion
    }
}
