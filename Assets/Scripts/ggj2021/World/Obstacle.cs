using pdxpartyparrot.Core;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Players
{
    [RequireComponent(typeof(Collider))]
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
