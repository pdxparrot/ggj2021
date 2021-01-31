using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class SheepBoundaryCollider : MonoBehaviour, IWorldBoundaryCollisionListener
    {
        [SerializeField]
        private Sheep _owner;

        #region IWorldBoundaryCollisionListener

        public void OnWorldBoundaryCollision(WorldBoundary boundary)
        {
            NPCManager.Instance.RespawnNPC(_owner, GameManager.Instance.GameGameData.SheepSpawnTag);
        }

        #endregion
    }
}
