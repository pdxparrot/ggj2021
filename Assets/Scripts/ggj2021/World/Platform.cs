using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Platform : Game.World.Platform
    {
        [SerializeField]
        private Transform _actorContainer;

        #region Unity Lifecycle

        protected override void OnDestroy()
        {
            // this may not actually be safe to do here
            for(int i = 0; i < _actorContainer.childCount; ++i) {
                Transform child = _actorContainer.GetChild(i);

                Player player = child.GetComponentInParent<Player>();
                if(null != player) {
                    OnPlayerExit(player);
                    continue;
                }

                Sheep sheep = child.GetComponentInParent<Sheep>();
                if(null != sheep) {
                    OnSheepExit(sheep);
                    continue;
                }
            }

            base.OnDestroy();
        }

        #endregion

        protected override void OnEnterPlatform(GameObject gameObject)
        {
            Player player = gameObject.GetComponentInParent<Player>();
            if(null != player) {
                OnPlayerEnter(player);
                return;
            }

            Sheep sheep = gameObject.GetComponentInParent<Sheep>();
            if(null != sheep) {
                OnSheepEnter(sheep);
                return;
            }
        }

        protected override void OnExitPlatform(GameObject gameObject)
        {
            Player player = gameObject.GetComponentInParent<Player>();
            if(null != player) {
                OnPlayerExit(player);
                return;
            }

            Sheep sheep = gameObject.GetComponentInParent<Sheep>();
            if(null != sheep) {
                OnSheepExit(sheep);
                return;
            }
        }

        private void OnPlayerEnter(Player player)
        {
            player.OnPlatformEnter(_actorContainer);
        }

        private void OnPlayerExit(Player player)
        {
            player.OnPlatformExit();
        }

        private void OnSheepEnter(Sheep sheep)
        {
            sheep.OnPlatformEnter(_actorContainer);
        }

        private void OnSheepExit(Sheep sheep)
        {
            sheep.OnPlatformExit();
        }
    }
}
