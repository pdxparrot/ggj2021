using System;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Interactables;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public sealed class Sheep : NPC25D, IInteractable
    {
        private SheepBehavior SheepBehavior => (SheepBehavior)NPCBehavior;

        public bool CanInteract => !SheepBehavior.IsCaught;

        public Type InteractableType => typeof(Sheep);

        private NavMeshObstacle Obstacle { get; set; }

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            Collider.isTrigger = true;

            GetComponent<AudioSource>().spatialBlend = 0.0f;

            // spawn as an obstacle
            Obstacle = GetComponent<NavMeshObstacle>();
            Obstacle.carving = true;

            // start with AI off
            EnableAgent(false);
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

        public void OnChambered(Transform parent)
        {
            transform.SetParent(parent);

            Model.gameObject.SetActive(false);

            // disable AI and obstacle
            EnableAgent(false);
            Obstacle.enabled = false;

            Rigidbody.isKinematic = true;

            SheepBehavior.OnChambered();
        }

        public void OnEnqueued(Transform target)
        {
            // enable AI, disable obstacle
            EnableAgent(true);
            Obstacle.enabled = false;

            SheepBehavior.OnEnqueued(target);
        }

        public void OnLaunch(Vector3 start, Vector3 direction)
        {
            Rigidbody.isKinematic = false;

            transform.SetParent(GameManager.Instance.BaseLevel.SheepPen);

            // disable AI and obstacle
            EnableAgent(false);
            Obstacle.enabled = false;

            SheepBehavior.OnLaunch(start, direction);

            Model.gameObject.SetActive(true);
        }

        #endregion
    }
}
