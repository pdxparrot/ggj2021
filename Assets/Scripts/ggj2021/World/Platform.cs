using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class Platform : Actor3D
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        private PlatformWaypoint _initialWaypoint;

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        [ReadOnly]
        private PlatformWaypoint _nextWaypoint;

        [SerializeReference]
        [ReadOnly]
        private ITimer _cooldown;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Initialize(Guid.NewGuid());

            Rigidbody.isKinematic = true;

            _cooldown = TimeManager.Instance.AddTimer();

            SetWaypoint(_initialWaypoint);
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_cooldown);
                _cooldown = null;
            }

            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if(null == _nextWaypoint || _cooldown.IsRunning) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused || !GameManager.Instance.IsGameReady || GameManager.Instance.IsGameOver) {
                return;
            }

            Movement.MoveTowards(_nextWaypoint.transform.position, _speed, dt);

            if(Vector3.Distance(Movement.Position, _nextWaypoint.transform.position) < float.Epsilon) {
                _cooldown.Start(_nextWaypoint.Cooldown);

                Movement.Position = _nextWaypoint.transform.position;
                SetWaypoint(_nextWaypoint.NextWaypoint);
            }
        }

        #endregion

        private void SetWaypoint(PlatformWaypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                Movement.Velocity = Vector3.zero;
                return;
            }
        }
    }
}
