using System;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.ggj2021.Camera;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    public sealed class Player : Player3D
    {
        private GameViewer PlayerGameViewer => (GameViewer)Viewer;

        [Space(10)]

        [SerializeField]
        private ShephardBehavior _shepherd;

        public ShephardBehavior Shepherd => _shepherd;

        [SerializeField]
        private ShephardModel _shephardModel;

        public ShephardModel ShephardModel => _shephardModel;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputHandler is PlayerInputHandler);

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
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

        public override void OnDeSpawn()
        {
            _shepherd.OnDeSpawn();

            PlayerGameViewer.FollowTarget(null);

            base.OnDeSpawn();
        }

        #endregion
    }
}
