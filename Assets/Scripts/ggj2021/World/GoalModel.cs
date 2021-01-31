using pdxpartyparrot.Core.Animation;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(SpineAnimationHelper))]
    public sealed class GoalModel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _goalScoredContainer;

        #region Unity Lifecycle

        private void Awake()
        {
            _goalScoredContainer.SetActive(false);
        }

        #endregion
    }
}
