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
                if(IsCaught || !Sheep.HasPath) {
                    return Vector3.zero;
                }

                Vector3 nextPosition = Sheep.NextPosition;
                return nextPosition - Owner.Movement.Position;
            }
        }

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        public bool IsCaught => _state == State.Chambered || _state == State.Enqueued || _state == State.Launched;

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
                NPCOwner.Stop(true, true);
                break;
            case State.Chambered:
                NPCOwner.Stop(true, false);
                break;
            case State.Enqueued:
                NPCOwner.Stop(true, false);
                break;
            case State.Launched:
                NPCOwner.Stop(true, false);
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
        }

        private void HandleLaunched()
        {
        }

        #endregion

        #region Event Handlers

        public void OnChambered()
        {
        }

        public void OnEnqueued(GameObject target)
        {
        }

        public void OnLaunch(Vector3 start, Vector3 direction)
        {
            transform.position = start;
        }

        #endregion
    }
}
