using System;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
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
            Carried,
            Enqueued,
            Launched,
        }

        private Sheep Sheep => (Sheep)Owner;

        private SheepBehaviorData SheepBehaviorData => (SheepBehaviorData)BehaviorData;

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

        public bool IsCarried => _state == State.Carried;

        public bool IsEnqueued => _state == State.Enqueued;

        public bool IsCaught => IsCarried || IsEnqueued;

        public bool IsLaunched => _state == State.Launched;

        [SerializeField]
        [ReadOnly]
        private Transform _target;

        #region Effects

        [SerializeField]
        private EffectTrigger _noiseEffect;

        #endregion

        [SerializeReference]
        [ReadOnly]
        private ITimer _noiseTimer;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            _noiseTimer = TimeManager.Instance.AddTimer();
            _noiseTimer.TimesUpEvent += NoiseTimesUpEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_noiseTimer);
            }

            _noiseTimer = null;
        }

        #endregion

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
            case State.Carried:
                HandleCarried();
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
                Sheep.SetObstacle();
                break;
            case State.Carried:
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

        private void HandleCarried()
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

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            //_noiseTimer.Start(SheepBehaviorData.NoiseFrequency);

            return false;
        }

        public override bool OnDeSpawn()
        {
            _noiseTimer.Stop();

            return base.OnDeSpawn();
        }

        #endregion

        #region Event Handlers

        private void NoiseTimesUpEventHandler(object sender, EventArgs args)
        {
            _noiseEffect.Trigger();

            _noiseTimer.Start(SheepBehaviorData.NoiseFrequency);
        }

        public void OnCarried()
        {
            _target = null;
            SetState(State.Carried);
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

        public void OnTeleport(Transform exitPoint)
        {
            // TODO: this isn't working right
            Owner.Movement.Teleport(exitPoint);
        }

        #endregion

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2021.SheepBehavior {Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }

        #endregion
    }
}
