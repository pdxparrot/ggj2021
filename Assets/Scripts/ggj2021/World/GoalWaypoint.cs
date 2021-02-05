using pdxpartyparrot.Core.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class GoalWaypoint : Waypoint
    {
        [SerializeField]
        private GoalWaypoint _nextWaypoint;

        public GoalWaypoint NextWaypoint => _nextWaypoint;

        [SerializeField]
        [Tooltip("The direction the goal should face while approaching this waypoint")]
        private Vector3 _goalFacing = new Vector3(0.0f, 0.0f, 1.0f);

        public Vector3 GoalFacing => _goalFacing;

        #region Unity Lifecycle

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Transform t = transform;
            Vector3 pos = t.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pos, pos + _goalFacing * 2.0f);
        }

        #endregion
    }
}
