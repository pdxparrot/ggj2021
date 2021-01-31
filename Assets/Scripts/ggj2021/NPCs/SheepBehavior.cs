using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2021.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.NPCs
{
    public sealed class SheepBehavior : NPCBehavior
    {
        private enum State
        {
            Idle,
            Chambered,
            Enqueued,
            Launched,
        }

        public Sheep Sheep => (Sheep)Owner;

        public override Vector3 MoveDirection
        {
            get
            {
                if(_state != State.Enqueued || !Sheep.HasPath) {
                    return Vector3.zero;
                }

                Vector3 nextPosition = Sheep.NextPosition;
                return (nextPosition - Owner.Movement.Position).normalized;
            }
        }

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        public bool IsCaught => _state == State.Chambered || _state == State.Enqueued;

        public bool IsChambered => _state == State.Chambered;

        public bool IsLaunched => _state == State.Launched;

        [SerializeField]
        [ReadOnly]
        private Transform _target;

        private float _lastFacingZ;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Sheep);
            Assert.IsTrue(behaviorData is SheepBehaviorData);

            base.Initialize(behaviorData);
        }

        public override bool OnThink(float dt)
        {
            switch(_state) {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chambered:
                HandleChambered();
                break;
            case State.Enqueued:
                HandleEnqueued();
                break;
            case State.Launched:
                HandleLaunched();
                break;
            }

            return true;
        }

        #region NPC State

        private void SetState(State state)
        {
            if(NPCManager.Instance.DebugBehavior) {
                Debug.Log($"Sheep {Owner.Id} set state {state}");
            }

            _state = state;
            switch(_state) {
            case State.Idle:
                Sheep.SetObstacle();
                break;
            case State.Chambered:
                Sheep.SetPassive();
                break;
            case State.Enqueued:
                Sheep.SetAgent();
                NPCOwner.Stop(true, false);
                break;
            case State.Launched:
                Sheep.SetPassive();
                break;
            }
        }

        private void HandleIdle()
        {
        }

        private void HandleChambered()
        {
        }

        private void HandleEnqueued()
        {
            if(null == _target) {
                SetState(State.Idle);
                return;
            }

            if(!Sheep.UpdatePath(_target.position)) {
                _target = null;
                return;
            }
        }

        private void HandleLaunched()
        {
            if(Owner.Movement.AtRest) {
                SetState(State.Idle);
            }
        }

        #endregion

        #region Event Handlers

        public void OnChambered()
        {
            _target = null;
            SetState(State.Chambered);
        }

        public void OnEnqueued(Transform target)
        {
            _target = target;
            SetState(State.Enqueued);
        }

        public void OnFree()
        {
            _target = null;
            SetState(State.Idle);
        }

        public void OnLaunch(Vector3 start, Vector3 direction)
        {
            Owner.Movement.Teleport(start);

            Vector3 velocity = direction * GameManager.Instance.GameGameData.SheepLaunchSpeed;
            Debug.Log($"Launching {velocity}");
            Owner.Movement.Velocity = velocity;

            SetState(State.Launched);
        }

        #endregion
    }
}
