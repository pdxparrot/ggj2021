using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    // TODO: use a WaypointFollower instead of having this handle that
    public sealed class Goal : Actor3D
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        private GoalWaypoint _initialWaypoint;

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        private string[] _tags;

        [SerializeField]
        private CompassNeedle _compassNeedlePrefab;

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

            GameManager.Instance.GameReadyEvent += GameReadyEventHandler;
            GameManager.Instance.GoalScoredEvent += GoalScoredEventHandler;

            SetWaypoint(_initialWaypoint);
        }

        protected override void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.GoalScoredEvent -= GoalScoredEventHandler;
                GameManager.Instance.GameReadyEvent -= GameReadyEventHandler;
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
                SetWaypoint(_nextWaypoint.NextGoalWaypoint);
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

        private bool CanScore(Sheep sheep)
        {
            if(null == sheep || !sheep.CanScore) {
                return false;
            }

            // if we have no tags, the sheep must have no tags
            // TODO: make this behavior configurable
            if(_tags.Length == 0) {
                return !sheep.HasTag;
            }

            // we have tags, so the sheep must also have tags
            // TODO: make this behavior configurable
            if(!sheep.HasTag) {
                return false;
            }

            // the sheep must have at least one of our tags
            return Array.Exists(_tags, x => x == sheep.Tag);
        }

        private void SetWaypoint(GoalWaypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                Movement.Velocity = Vector3.zero;
                return;
            }

            transform.forward = _nextWaypoint.GoalFacing;
            SetFacing(transform.forward);

            _goalModel.RotateBillboards();
        }

        private void LaunchSheep(Sheep sheep)
        {
            // TODO: this doesn't line up with the animation *at all*

            Debug.Log($"Goal launching sheep {sheep.Id}");

            Vector3 direction = (FacingDirection + new Vector3(0.0f, 1.0f, 0.0f)).normalized;
            sheep.OnLaunch(sheep.Movement.Position, direction);
        }

        #region Event Handlers

        private void GameReadyEventHandler(object sender, EventArgs args)
        {
            GameUIManager.Instance.GameGameUI.PlayerHUD.AddCompassNeedle(_compassNeedlePrefab, this);
        }

        private void OnSheepTrigger(Sheep sheep)
        {
            if(!CanScore(sheep)) {
                if(sheep.CanScore) {
                    LaunchSheep(sheep);
                }
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
