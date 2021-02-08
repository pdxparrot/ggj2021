using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2021.Level
{
    public sealed class MainLevel : LevelHelper, IBaseLevel
    {
        [SerializeField]
        private Key _cheatKey = Key.T;

        [SerializeField]
        private float _roundSeconds = 60.0f;

        [SerializeField]
        [Tooltip("Maximum number of sheep to spawn. 0 means spawn 1 per-spawnpoint. Will never spawn more than there are spawnpoints.")]
        private int _maxSheep;

        [SerializeField]
        [ReadOnly]
        private bool _started;

        [SerializeField]
        [ReadOnly]
        private bool _levelStarted;

        [SerializeReference]
        [ReadOnly]
        private ITimer _timer;

        [SerializeField]
        [ReadOnly]
        private float _lastTimePercent;

        public float TimePercent => _lastTimePercent;

        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _sheepPen;

        [CanBeNull]
        public Transform SheepPen => null == _sheepPen ? null : _sheepPen.transform;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _timer = TimeManager.Instance.AddTimer();
            _timer.TimesUpEvent += LevelTimesUpEventHandler;
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_timer);
                _timer = null;
            }

            base.OnDestroy();
        }

        private void Update()
        {
            _lastTimePercent = _levelStarted ? 1.0f - (_timer.SecondsRemaining / _roundSeconds) : 0;
            if(GameManager.Instance.IsGameReady && null != GameUIManager.Instance.GameGameUI) {
                GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateTimer(1.0f - _lastTimePercent);
            }
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            if(Keyboard.current[_cheatKey].wasPressedThisFrame) {
                while(GameManager.Instance.Score < GameManager.Instance.Goal) {
                    if(!GameManager.Instance.OnGoalScored()) {
                        break;
                    }
                }
            }
#endif
        }

        #endregion

        private void SpawnSheep()
        {
            int spawnCount = SpawnManager.Instance.SpawnPointCount(GameManager.Instance.GameGameData.SheepSpawnTag);
            int sheepCount = _maxSheep <= 0 ? spawnCount : Mathf.Min(_maxSheep, spawnCount);

            Debug.Log($"Spawning {sheepCount} sheep");

            for(int i = 0; i < sheepCount; ++i) {
                SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
                spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData, _sheepPen.transform);
            }
        }

        #region Event Handlers

        protected override void GameStartClientEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.Reset(0);

            base.GameStartClientEventHandler(sender, args);
        }

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            GameManager.Instance.RoundWonEvent += RoundWonEventHandler;

            Assert.IsNull(_sheepPen);
            _sheepPen = new GameObject("Sheep");

            SpawnSheep();

            GameManager.Instance.Reset(NPCManager.Instance.NPCs.Count);

            _timer.Start(_roundSeconds);
            _levelStarted = true;

            GameManager.Instance.OnLevelEntered();
        }

        protected override void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.RoundWonEvent -= RoundWonEventHandler;

            base.GameUnReadyEventHandler(sender, args);
        }

        protected override void LevelTransitioningEventHandler(object sender, EventArgs args)
        {
            if(null != _sheepPen) {
                Destroy(_sheepPen);
                _sheepPen = null;
            }

            base.LevelTransitioningEventHandler(sender, args);
        }

        private void LevelTimesUpEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Times up!");

            GameManager.Instance.GameOver();
        }

        private void RoundWonEventHandler(object sender, EventArgs args)
        {
            _timer.Stop();

            Debug.Log("You win this round!");

            TransitionLevel();
        }

        #endregion
    }
}
