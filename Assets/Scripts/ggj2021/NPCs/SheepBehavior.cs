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
        }

        public Sheep Sheep => (Sheep)Owner;

        public override Vector3 MoveDirection
        {
            get
            {
                if(!Sheep.HasPath) {
                    return Vector3.zero;
                }

                Vector3 nextPosition = Sheep.NextPosition;
                return nextPosition - Owner.Movement.Position;
            }
        }

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

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
                NPCOwner.Stop(true, false);
                break;
            }
        }

        private void HandleIdle()
        {
        }

        #endregion
    }
}
