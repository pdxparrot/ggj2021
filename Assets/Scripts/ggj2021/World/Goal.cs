using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Goal : MonoBehaviour
    {
        private Collider _collider;

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.layer = GameManager.Instance.GameGameData.GoalLayer;

            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if(null == sheep || !sheep.CanScore) {
                return;
            }

            Debug.Log("Goooooaaaalllll!");

            sheep.OnScored();
        }

        #endregion
    }
}
