using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2021.NPCs;
using pdxpartyparrot.ggj2021.UI;
using pdxpartyparrot.ggj2021.World;

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
        private GoalWaypoint _initialGoalWaypoint;

        [SerializeField]
        private float _goalSpeed = 5.0f;

        [SerializeField]
        [Tooltip("Maximum number of sheep to spawn. 0 means spawn 1 per-spawnpoint. Will never spawn more than there are spawnpoints.")]
        private int _maxSheep;

        [SerializeReference]
        [ReadOnly]
        private ITimer _timer;

        [CanBeNull]
        private GameObject _sheepPen;

        [CanBeNull]
        private Goal _goal;

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
            if(null != GameUIManager.Instance.GameGameUI) {
                GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateTimer(_timer.SecondsRemaining / _roundSeconds);
            }
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            if(Keyboard.current[_cheatKey].wasPressedThisFrame) {
                while(GameManager.Instance.Score < GameManager.Instance.Goal) {
                    GameManager.Instance.OnGoalScored();
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

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            GameManager.Instance.RoundWonEvent += RoundWonEventHandler;

            Assert.IsNull(_sheepPen);
            _sheepPen = new GameObject("Sheep Pen");

            SpawnSheep();

            GameManager.Instance.Reset(NPCManager.Instance.NPCs.Count);

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.GoalSpawnTag);
            _goal = spawnPoint.SpawnFromPrefab(GameManager.Instance.GameGameData.GoalPrefab, null) as Goal;
            _goal.Initialize(_initialGoalWaypoint, _goalSpeed);

            _timer.Start(_roundSeconds);
        }

        protected override void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.RoundWonEvent -= RoundWonEventHandler;

            Destroy(_sheepPen);
            _sheepPen = null;

            Destroy(_goal.gameObject);
            _goal = null;

            base.GameUnReadyEventHandler(sender, args);
        }

        private void LevelTimesUpEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Times up!");

            GameManager.Instance.GameOver();
        }

        private void RoundWonEventHandler(object sender, EventArgs args)
        {
            _timer.Stop();

            Debug.Log("You win!");

            TransitionLevel();
        }

        #endregion
    }
}
