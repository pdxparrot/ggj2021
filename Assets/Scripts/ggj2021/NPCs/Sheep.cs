using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2021.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class Sheep : NPC25D, IInteractable
    {
        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private SheepData _sheepData;

        public SheepData SheepData => _sheepData;

        public bool CanInteract => true;//!IsHeld;

        public Type InteractableType => typeof(Sheep);

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is SheepBehavior);
        }

        #endregion

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
    }
}
