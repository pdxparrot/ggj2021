using pdxpartyparrot.Core;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.Game.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class StaticObstacle : MonoBehaviour
    {
        private Collider Collider { get; set; }

        private NavMeshObstacle NavMeshObstacle { get; set; }

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.layer = GameStateManager.Instance.GameManager.GameData.WorldLayer;

            Collider = GetComponent<Collider>();
            Collider.sharedMaterial = PartyParrotManager.Instance.FrictionlessMaterial;

            NavMeshObstacle = GetComponent<NavMeshObstacle>();
            NavMeshObstacle.carving = true;
        }

        #endregion
    }
}
