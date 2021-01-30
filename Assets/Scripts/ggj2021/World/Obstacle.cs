using pdxpartyparrot.Core;

using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public sealed class Obstacle : MonoBehaviour
    {
        private Collider Collider { get; set; }

        private NavMeshObstacle NavMeshObstacle { get; set; }

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.layer = GameManager.Instance.GameData.WorldLayer;

            Collider = GetComponent<Collider>();
            Collider.sharedMaterial = PartyParrotManager.Instance.FrictionlessMaterial;

            NavMeshObstacle = GetComponent<NavMeshObstacle>();
            NavMeshObstacle.carving = true;
        }

        #endregion
    }
}
