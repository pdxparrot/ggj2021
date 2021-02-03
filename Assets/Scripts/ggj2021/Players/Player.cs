using System;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2021.Camera;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    public sealed class Player : Player3D, IWorldBoundaryCollisionListener
    {
        public PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        private GameViewer PlayerGameViewer => (GameViewer)Viewer;

        [Space(10)]

        [SerializeField]
        private ShephardBehavior _shepherd;

        public ShephardBehavior Shepherd => _shepherd;

        [SerializeField]
        private ShephardModel _shephardModel;

        public ShephardModel ShephardModel => _shephardModel;

        #region Effects

        [SerializeField]
        private EffectTrigger _fallOutEffect;

        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputHandler is PlayerInputHandler);

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
        }

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;

            _shepherd.Initialize();

            return true;
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            if(!_shepherd.OnSpawn(spawnpoint)) {
                return false;
            }

            PlayerGameViewer.FollowTarget(gameObject);

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            if(!_shepherd.OnReSpawn(spawnpoint)) {
                return false;
            }

            PlayerGameViewer.FollowTarget(gameObject);

            return true;
        }

        public override void OnDeSpawn()
        {
            _shepherd.OnDeSpawn();

            PlayerGameViewer.FollowTarget(null);

            base.OnDeSpawn();
        }

        #endregion

        #region IWorldBoundaryCollisionListener

        public void OnWorldBoundaryCollisionEnter(WorldBoundary boundary)
        {
            _fallOutEffect.Trigger();
        }

        public void OnWorldBoundaryCollisionExit(WorldBoundary boundary)
        {
            PlayerManager.Instance.RespawnPlayerNearest(this, PlayerManager.Instance.GamePlayerData.RespawnTag);
        }

        #endregion
    }
}
