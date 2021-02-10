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
        private GameObject[] _billboards;

        [SerializeField]
        private float _defaultRotation = 180.0f;

        #region Unity Lifecycle

        private void Awake()
        {
            _goalScoredContainer.SetActive(false);
        }

        #endregion

        public void RotateBillboards()
        {
            RotateBillboard(_goalScoredContainer);

            foreach(GameObject billboard in _billboards) {
                RotateBillboard(billboard);
            }
        }

        private void RotateBillboard(GameObject billboard)
        {
            Vector3 rot = billboard.transform.eulerAngles;
            rot.y = _defaultRotation;
            billboard.transform.eulerAngles = rot;
        }
    }
}
