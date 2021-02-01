using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.UI;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2021.Players
{
    // TODO: make an ActorComponent
    [RequireComponent(typeof(Interactables3D))]
    public sealed class ShephardBehavior : MonoBehaviour
    {
        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        [Space(10)]

        #region Sheep

        [SerializeField]
        private Transform _chamberParent;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Sheep _chamber;

        public bool CarryingSheep => null != _chamber;

        [SerializeField]
        [ReadOnly]
        private List<Sheep> _magazine = new List<Sheep>();

        public bool HasCapacity => null == _chamber || (GameManager.HasInstance && _magazine.Count < GameManager.Instance.GameGameData.MaxQueuedSheep);

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

        [SerializeField]
        [ReadOnly]
        private ITimer _launchCooldown;

        public bool CanLaunch => null != _chamber && !_launchCooldown.IsRunning;

        private Interactables _interactables;

        #region Unity Lifecycle

        private void Awake()
        {
            _interactables = GetComponent<Interactables>();

            _launchCooldown = TimeManager.Instance.AddTimer();

            GameManager.Instance.GameUnReadyEvent += GameUnReadyEventHandler;
            GameManager.Instance.LevelEnterEvent += LevelEnterEventHandler;
            GameManager.Instance.GoalScoredEvent += GoalScoredEventHandler;
            GameManager.Instance.RoundWonEvent += RoundWonEventHandler;
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
                if(null != GameManager.Instance.BaseLevel.Goal) {
                    Vector3 d = GameManager.Instance.BaseLevel.Goal.transform.position - transform.position;
                    float angle = Vector3.SignedAngle(Vector3.forward, d, Vector3.up);
                    GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateGoalCompass(angle);
                } else {
                    GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateGoalCompass(0.0f);
                }
            }
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.RoundWonEvent -= RoundWonEventHandler;
                GameManager.Instance.GoalScoredEvent -= GoalScoredEventHandler;
                GameManager.Instance.LevelEnterEvent -= LevelEnterEventHandler;
                GameManager.Instance.GameUnReadyEvent -= GameUnReadyEventHandler;
            }

            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_launchCooldown);
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
            if(null == sheep || sheep.CanScore) {
                return false;
            }

            Debug.Log($"Caught sheep {sheep.Id}");

            if(null == _chamber) {
                ChamberSheep(sheep);

                _grabEffect.Trigger();
            } else {
                EnqueueSheep(sheep);
            }

            GameManager.Instance.OnSheepCollected();

            _interactables.RemoveInteractable(sheep);

            return true;
        }

        private void ChamberSheep(Sheep sheep)
        {
            Assert.IsNull(_chamber);

            _chamber = sheep;
            sheep.OnChambered(_chamberParent);

            Debug.Log($"Chambered sheep {_chamber.Id}");

            Owner.GamePlayerBehavior.OnCarryingSheepChanged();
        }

        private void EnqueueSheep(Sheep sheep)
        {
            Assert.IsTrue(HasCapacity);

            _magazine.Add(sheep);

            if(_magazine.Count == 1) {
                sheep.OnEnqueued(Owner.transform);
            } else {
                sheep.OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _magazine.ElementAt(_magazine.Count - 2).transform);
            }
        }

        public bool LaunchSheep()
        {
            if(!CanLaunch) {
                return false;
            }

            Sheep sheep = _chamber;
            _chamber = null;

            Debug.Log($"Launching sheep {sheep.Id}");

            _launchEffect.Trigger();

            Vector3 direction = (Owner.FacingDirection + new Vector3(0.0f, 1.0f, 0.0f)).normalized;
            sheep.OnLaunch(Owner.Movement.Position, direction);

            GameManager.Instance.OnSheepLost();

            CycleRound();

            _launchCooldown.Start(Owner.GamePlayerBehavior.GamePlayerBehaviorData.LaunchCooldown);

            Owner.GamePlayerBehavior.OnCarryingSheepChanged();

            return true;
        }

        private bool CycleRound()
        {
            if(_magazine.Count < 1) {
                return false;
            }

            Sheep sheep = _magazine.ElementAt(0);
            _magazine.RemoveAt(0);

            ChamberSheep(sheep);

            RackSheep();

            return true;
        }

        private void RackSheep()
        {
            if(_magazine.Count < 1) {
                return;
            }

            _magazine.ElementAt(0).OnEnqueued(Owner.transform);
            for(int i = 1; i < _magazine.Count; ++i) {
                _magazine.ElementAt(i).OnEnqueued(GameManager.Instance.GameGameData.SheepTargetPlayer
                    ? Owner.transform
                    : _magazine.ElementAt(i - 1).transform);
            }
        }

        private void FreeSheep()
        {
            // we get to keep the chambered sheep

            foreach(Sheep sheep in _magazine) {
                sheep.OnFree();

                GameManager.Instance.OnSheepLost();
            }
            _magazine.Clear();
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
            // TODO: if there's ever some other "level cleanup" event
            // that's where we should be doing this instead of on unready

            if(null != _chamber) {
                Destroy(_chamber.gameObject);
                _chamber = null;
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

        #endregion
    }
}
