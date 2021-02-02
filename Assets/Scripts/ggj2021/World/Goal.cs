using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class Goal : Actor3D
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        private GoalWaypoint _initialWaypoint;

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        [ReadOnly]
        private GoalWaypoint _nextWaypoint;

        #region Effects

        [SerializeField]
        private EffectTrigger _goalScoredEffect;

        #endregion

        private GoalModel _goalModel;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Initialize(Guid.NewGuid());

            GoalManager.Instance.RegisterGoal(this);

            _goalModel = Model.GetComponent<GoalModel>();

            gameObject.layer = GameManager.Instance.GameGameData.GoalLayer;

            Rigidbody.isKinematic = true;
            Collider.isTrigger = true;

            GameManager.Instance.GoalScoredEvent += GoalScoredEventHandler;

            SetWaypoint(_initialWaypoint);
        }

        protected override void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.GoalScoredEvent -= GoalScoredEventHandler;
            }

            if(GoalManager.HasInstance) {
                GoalManager.Instance.UnRegisterGoal(this);
            }

            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if(null == _nextWaypoint) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused || !GameManager.Instance.IsGameReady || GameManager.Instance.IsGameOver) {
                return;
            }

            Movement.MoveTowards(_nextWaypoint.transform.position, _speed, dt);

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

        private void SetWaypoint(GoalWaypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                Movement.Velocity = Vector3.zero;
                return;
            }

            transform.forward = _nextWaypoint.GoalFacing;
            _goalModel.RotateGoalScored();
        }

        #region Event Handlers

        private void OnSheepTrigger(Sheep sheep)
        {
            if(!sheep.CanScore) {
                return;
            }

            sheep.OnScored();

            GameManager.Instance.OnGoalScored();
        }

        private void GoalScoredEventHandler(object sender, EventArgs args)
        {
            _goalScoredEffect.Trigger();
        }

        #endregion
    }
}
