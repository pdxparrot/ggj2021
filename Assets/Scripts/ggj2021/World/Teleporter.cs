using pdxpartyparrot.ggj2021.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class Teleporter : Game.World.Teleporter
    {
        [SerializeField]
        private bool _sheepFollow = true;

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
