using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class Goal : Actor3D
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        private GoalWaypoint _nextWaypoint;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            gameObject.layer = GameManager.Instance.GameGameData.GoalLayer;

            Rigidbody.isKinematic = true;
            Collider.isTrigger = true;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if(null == _nextWaypoint) {
                return;
            }

            Movement.MoveTowards(_nextWaypoint.transform.position, GameManager.Instance.GameGameData.GoalSpeed, dt);

            if(Vector3.Distance(Movement.Position, _nextWaypoint.transform.position) < float.Epsilon) {
                Movement.Position = _nextWaypoint.transform.position;
                SetWaypoint(_nextWaypoint.NextWaypoint);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if(null != sheep) {
                OnSheepTrigger(sheep);
                return;
            }
        }

        #endregion

        public void SetWaypoint(GoalWaypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                Movement.Velocity = Vector3.zero;
                return;
            }

            transform.forward = _nextWaypoint.GoalFacing;
        }

        #region Event Handlers

        private void OnSheepTrigger(Sheep sheep)
        {
            if(!sheep.CanScore) {
                return;
            }

            GameManager.Instance.OnGoalScored();

            sheep.OnScored();
        }

        #endregion
    }
}
