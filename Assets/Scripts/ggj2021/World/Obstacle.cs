using pdxpartyparrot.Core;

using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.ggj2021.Players
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public sealed class Obstacle : MonoBehaviour
    {
        public Collider Collider { get; private set; }

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.layer = GameManager.Instance.GameData.WorldLayer;

            Collider = GetComponent<Collider>();
            Collider.sharedMaterial = PartyParrotManager.Instance.FrictionlessMaterial;
        }

        #endregion
    }
}
