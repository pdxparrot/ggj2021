using pdxpartyparrot.Core;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(Collider))]
    public sealed class SheepPhysics : MonoBehaviour, IWorldBoundaryCollisionListener
    {
        [SerializeField]
        private Sheep _owner;

        private Collider _collider;

        #region Unity Lifecycle

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.sharedMaterial = PartyParrotManager.Instance.FrictionlessMaterial;
        }

        #endregion

        #region IWorldBoundaryCollisionListener

        public void OnWorldBoundaryCollision(WorldBoundary boundary)
        {
            NPCManager.Instance.RespawnNPC(_owner, GameManager.Instance.GameGameData.SheepSpawnTag);
        }

        #endregion
    }
}
