using System;

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
        private SheepBehavior SheepBehavior => (SheepBehavior)NPCBehavior;

        public bool CanInteract => !SheepBehavior.IsCaught;

        public Type InteractableType => typeof(Sheep);

        public bool CanScore => SheepBehavior.IsLaunched;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            Collider.isTrigger = true;

            GetComponent<AudioSource>().spatialBlend = 0.0f;

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
            if(SheepBehavior.IsChambered) {
                transform.localPosition = Vector3.zero;
            }
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is SheepBehavior);
        }

        private void SetChambered(Transform parent, bool chambered)
        {
            transform.SetParent(parent);

            Model.gameObject.SetActive(!chambered);

            Rigidbody.isKinematic = chambered;

            if(chambered) {
                Movement.Position = parent.position;
            }
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            NPCManager.Instance.RegisterNPC(this);

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

        public void OnChambered(Transform parent)
        {
            SetChambered(parent, true);

            SheepBehavior.OnChambered();
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
            SetChambered(GameManager.Instance.BaseLevel.SheepPen, false);

            SheepBehavior.OnLaunch(start, direction);
        }

        public void OnScored()
        {
            DeSpawn(true);
        }

        #endregion
    }
}
