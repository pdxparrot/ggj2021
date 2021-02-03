using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2021.Data.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    [RequireComponent(typeof(ShephardBehavior))]
    public sealed class PlayerBehavior : Game.Characters.Players.PlayerBehavior
    {
        public PlayerBehaviorData GamePlayerBehaviorData => (PlayerBehaviorData)PlayerBehaviorData;

        [SerializeField]
        private EffectTrigger _carryingIdleEffect;

        [SerializeField]
        private EffectTrigger _carryingMovingEffectTrigger;

        private ShephardBehavior _shepherdBehavior;

        public ShephardBehavior ShepherdBehavior => _shepherdBehavior;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _shepherdBehavior = GetComponent<ShephardBehavior>();
        }

        #endregion

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Player);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);
        }

        protected override void TriggerIdle()
        {
            if(_shepherdBehavior.CarryingSheep) {
                _carryingIdleEffect.Trigger();
            } else {
                IdleEffect.Trigger();
            }
        }

        protected override void TriggerMoving()
        {
            if(_shepherdBehavior.CarryingSheep) {
                _carryingMovingEffectTrigger.Trigger();
            } else {
                MovingEffectTrigger.Trigger();
            }
        }

        #region Event Handlers

        public void OnCarryingSheepChanged()
        {
            TriggerMoveEffect();
        }

        #endregion
    }
}
