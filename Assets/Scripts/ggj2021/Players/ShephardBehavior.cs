using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.UI;
using pdxpartyparrot.ggj2021.World;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    // TODO: make an ActorComponent
    [RequireComponent(typeof(Interactables3D))]
    public sealed class ShephardBehavior : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Grabbing,
            Launching,
        }

        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        [Space(10)]

        #region Sheep

        [SerializeField]
        private Transform _sheepParent;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Sheep _carrying;

        [SerializeField]
        [ReadOnly]
        private List<Sheep> _queue = new List<Sheep>();

        #endregion

        #region Effects

        [SerializeField]
        private EffectTrigger _levelEnteredEffect;

        [SerializeField]
        private EffectTrigger _goalScoredEffect;

        [SerializeField]
        private EffectTrigger _roundWonEffect;

        [SerializeField]
        private EffectTrigger _grabEffect;

        [SerializeField]
        private EffectTrigger _launchEffect;

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        #endregion

        [SerializeField]
        private GameObject _aimer;

        public bool CarryingSheep => null != _carrying;

        private bool HasCapacity => null == _carrying || (GameManager.HasInstance && _queue.Count < GameManager.Instance.GameGameData.MaxQueuedSheep);

        private bool CanGrab => null == _carrying && _state == State.Idle;

        private bool CanLaunch => null != _carrying && _state == State.Idle;

        [SerializeReference]
        [ReadOnly]
        private ITimer _teleportTimer;

        public bool CanTeleport => !_teleportTimer.IsRunning;

        private Interactables _interactables;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        private void Awake()
        {
            _interactables = GetComponent<Interactables>();

            GameManager.Instance.GameUnReadyEvent += GameUnReadyEventHandler;
            GameManager.Instance.LevelTransitioningEvent += LevelTransitioningEventHandler;
            GameManager.Instance.LevelEnterEvent += LevelEnterEventHandler;
            GameManager.Instance.GoalScoredEvent += GoalScoredEventHandler;
            GameManager.Instance.RoundWonEvent += RoundWonEventHandler;

            _teleportTimer = TimeManager.Instance.AddTimer();

            InitDebugMenu();
        }

        private void Update()
        {
            if(!GameManager.Instance.IsGameReady) {
                return;
            }

            // TODO: not sure why this needs to be negative, tbh
            _aimer.transform.forward = -Owner.FacingDirection;

            CatchSheep();

            if(null != GameUIManager.Instance.GameGameUI) {
                Goal goal = GoalManager.Instance.GetNearestGoal(transform);
                if(null != goal) {
                    Vector3 d = goal.transform.position - transform.position;
                    float angle = Vector3.SignedAngle(Vector3.forward, d, Vector3.up);
                    GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateGoalCompass(angle);
                } else {
                    GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateGoalCompass(0.0f);
                }
            }
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();

            if(GameManager.HasInstance) {
                GameManager.Instance.RoundWonEvent -= RoundWonEventHandler;
                GameManager.Instance.GoalScoredEvent -= GoalScoredEventHandler;
                GameManager.Instance.LevelEnterEvent -= LevelEnterEventHandler;
                GameManager.Instance.GameUnReadyEvent -= GameUnReadyEventHandler;
                GameManager.Instance.LevelTransitioningEvent -= LevelTransitioningEventHandler;
            }

            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_teleportTimer);
                _teleportTimer = null;
            }
        }

        #endregion

        public void Initialize()
        {
            foreach(RumbleEffectTriggerComponent rumble in _rumbleEffects) {
                rumble.PlayerInput = Owner.PlayerInputHandler.InputHelper;
            }
        }

        #region Sheep

        public bool CatchSheep()
        {
            if(!HasCapacity) {
                return false;
            }

            Sheep sheep = _interactables.GetRandomInteractable<Sheep>();
            if(null == sheep || !sheep.CanInteract) {
                return false;
            }

            if(null == _carrying) {
                if(!GrabSheep(sheep)) {
                    return false;
                }
            } else if(!sheep.IsInQueue) {
                EnqueueSheep(sheep);
            } else {
                return false;
            }

            Debug.Log($"Caught sheep {sheep.Id}");

            GameManager.Instance.OnSheepCollected();

            _interactables.RemoveInteractable(sheep);

            return true;
        }

        private bool GrabSheep(Sheep sheep)
        {
            if(!CanGrab) {
                return false;
            }

            DequeueSheep(sheep);
            _carrying = sheep;

            sheep.OnCarried(_sheepParent);

            _state = State.Grabbing;
            _grabEffect.Trigger(() => {
                _state = State.Idle;

                Debug.Log($"Grabbing sheep {sheep.Id}");

                Owner.GamePlayerBehavior.OnCarryingSheepChanged();
            });

            return true;
        }

        private void EnqueueSheep(Sheep sheep)
        {
            Assert.IsTrue(_queue.Count < GameManager.Instance.GameGameData.MaxQueuedSheep);
            Assert.IsFalse(_queue.Contains(sheep));

            _queue.Add(sheep);

            if(_queue.Count == 1) {
                sheep.OnEnqueued(Owner.transform);
            } else {
                sheep.OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _queue.ElementAt(_queue.Count - 2).transform);
            }
        }

        private void DequeueSheep(Sheep sheep)
        {
            if(!_queue.Remove(sheep)) {
                return;
            }

            if(_queue.Count < 1) {
                return;
            }

            _queue.ElementAt(0).OnEnqueued(Owner.transform);
            for(int i = 1; i < _queue.Count; ++i) {
                _queue.ElementAt(i).OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _queue.ElementAt(i - 1).transform);
            }
        }

        public bool LaunchSheep()
        {
            if(!CanLaunch) {
                return false;
            }

            Sheep sheep = _carrying;
            _carrying = null;

            // TODO: this doesn't line up with the animation *at all*

            Debug.Log($"Launching sheep {sheep.Id}");

            Vector3 direction = (Owner.FacingDirection + new Vector3(0.0f, 1.0f, 0.0f)).normalized;
            sheep.OnLaunch(Owner.Movement.Position, direction);

            GameManager.Instance.OnSheepLost();

            _state = State.Launching;
            _launchEffect.Trigger(() => {
                _state = State.Idle;

                Owner.GamePlayerBehavior.OnCarryingSheepChanged();
            });

            return true;
        }

        private void FreeSheep()
        {
            // we get to keep the sheep we're carrying

            foreach(Sheep sheep in _queue) {
                sheep.OnFree();

                GameManager.Instance.OnSheepLost();
            }
            _queue.Clear();
        }

        #endregion

        #region Spawn

        public bool OnSpawn(SpawnPoint spawnpoint)
        {
            return true;
        }

        public bool OnReSpawn(SpawnPoint spawnpoint)
        {
            FreeSheep();

            return true;
        }

        public void OnDeSpawn()
        {
            FreeSheep();
        }

        #endregion

        #region Event Handlers

        private void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            FreeSheep();
        }

        private void LevelTransitioningEventHandler(object sender, EventArgs args)
        {
            if(null != _carrying) {
                Destroy(_carrying.gameObject);
                _carrying = null;
            }
        }

        private void LevelEnterEventHandler(object sender, EventArgs args)
        {
            _levelEnteredEffect.Trigger();
        }

        private void GoalScoredEventHandler(object sender, EventArgs args)
        {
            _goalScoredEffect.Trigger();
        }

        private void RoundWonEventHandler(object sender, EventArgs args)
        {
            _roundWonEffect.Trigger();
        }

        public void OnTeleport(Transform exitPoint, bool sheepFollow)
        {
            Owner.Movement.Teleport(exitPoint);

            if(sheepFollow) {
                // TODO: this would be cooler if they path'd
                // to the teleporter and then went through
                foreach(Sheep sheep in _queue) {
                    Debug.Log($"Sheep {sheep.Id} following");
                    sheep.SheepBehavior.OnTeleport(exitPoint);
                }
            } else {
                FreeSheep();
            }

            _teleportTimer.Start(Owner.GamePlayerBehavior.GamePlayerBehaviorData.TeleportCooldown);
        }

        #endregion

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2021.ShephardBehavior {Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Carrying sheep: {CarryingSheep}");
                GUILayout.Label($"Has capacity: {HasCapacity}");
                GUILayout.Label($"Can grab sheep: {CanGrab}");
                GUILayout.Label($"Can launch sheep: {CanLaunch}");
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
