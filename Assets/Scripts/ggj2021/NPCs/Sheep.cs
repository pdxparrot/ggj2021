using System;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Interactables;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class Sheep : NPC25D, IInteractable
    {
        private SheepBehavior SheepBehavior => (SheepBehavior)NPCBehavior;

        public bool CanInteract => !SheepBehavior.IsCaught;

        public Type InteractableType => typeof(Sheep);

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            Collider.isTrigger = true;

            GetComponent<AudioSource>().spatialBlend = 0.0f;
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is SheepBehavior);
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

        public void OnChambered()
        {
            Model.gameObject.SetActive(false);
            EnableAgent(false);

            SheepBehavior.OnChambered();
        }

        public void OnEnqueued(GameObject target)
        {
            SheepBehavior.OnEnqueued(target);
        }

        public void OnLaunch(Vector3 start, Vector3 direction)
        {
            SheepBehavior.OnLaunch(start, direction);

            Model.gameObject.SetActive(true);
        }

        #endregion
    }
}
