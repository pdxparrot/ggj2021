using pdxpartyparrot.ggj2021.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class Teleporter : Game.World.Teleporter
    {
        [SerializeField]
        private bool _sheepFollow;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            // TODO: disable for now, following doesn't work
            if(_sheepFollow) {
                Debug.LogWarning("Disabling sheep follow");
                _sheepFollow = false;
            }
        }

        #endregion

        protected override bool CanTeleport(GameObject gameObject)
        {
            Player player = gameObject.GetComponentInParent<Player>();
            if(null == player || !player.GamePlayerBehavior.ShepherdBehavior.CanTeleport) {
                return false;
            }

            return true;
        }

        protected override void OnTeleport(GameObject gameObject, Game.World.Teleporter source)
        {
            Player player = gameObject.GetComponentInParent<Player>();
            if(null == player) {
                return;
            }

            Teleporter gameSource = (Teleporter)source;
            player.GamePlayerBehavior.ShepherdBehavior.OnTeleport(ExitPoint, gameSource._sheepFollow);
        }
    }
}
