using pdxpartyparrot.Core.Animation;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    [RequireComponent(typeof(SpineAnimationHelper))]
    public sealed class GoalModel : MonoBehaviour
    {
        [SerializeField]
        private Goal _owner;

        [SerializeField]
        private GameObject _goalScoredContainer;

        [SerializeField]
        private float _defaultRotation = 180.0f;

        #region Unity Lifecycle

        private void Awake()
        {
            _goalScoredContainer.SetActive(false);
        }

        #endregion

        public void RotateGoalScored()
        {
            Vector3 rot = _goalScoredContainer.transform.eulerAngles;
            rot.y = _defaultRotation;
            _goalScoredContainer.transform.eulerAngles = rot;
        }
    }
}
