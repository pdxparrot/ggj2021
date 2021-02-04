using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2021.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Teleporter : MonoBehaviour
    {
        [SerializeField]
        private Teleporter _exit;

        [SerializeField]
        private Transform _exitPoint;

        [SerializeField]
        private bool _sheepFollow;

        #region Effects

        [SerializeField]
        private EffectTrigger _enterEffect;

        [SerializeField]
        private EffectTrigger _exitEffect;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;

            // only allow rotating around the y axis
            Vector3 rot = _exitPoint.eulerAngles;
            rot.y = 0.0f;
            rot.z = 0.0f;
            _exitPoint.eulerAngles = rot;

            // TODO: disable for now, following doesn't work
            if(_sheepFollow) {
                Debug.LogWarning("Disabling sheep follow");
                _sheepFollow = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();
            if(null == player || !player.GamePlayerBehavior.ShepherdBehavior.CanTeleport) {
                return;
            }

            _enterEffect.Trigger();

            _exit.Teleport(player);
        }

        #endregion

        private void Teleport(Player player)
        {
            player.GamePlayerBehavior.ShepherdBehavior.OnTeleport(_exitPoint, _sheepFollow);

            _exitEffect.Trigger();
        }
    }
}
