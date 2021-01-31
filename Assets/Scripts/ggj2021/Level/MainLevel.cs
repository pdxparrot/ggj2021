using System;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2021.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Level
{
    public sealed class MainLevel : LevelHelper, IBaseLevel
    {
        [SerializeField]
        private GoalWaypoint _initialGoalWaypoint;

        [SerializeField]
        private float _roundSeconds = 60.0f;

        [SerializeReference]
        [ReadOnly]
        private ITimer _timer;

        private GameObject _sheepPen;

        private Goal _goal;

        public Transform SheepPen => _sheepPen.transform;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _sheepPen = new GameObject("Sheep Pen");

            _timer = TimeManager.Instance.AddTimer();
            _timer.TimesUpEvent += LevelTimesUpEventHandler;
        }

        protected override void OnDestroy()
        {
            Destroy(_sheepPen);
            Destroy(_goal.gameObject);

            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_timer);
                _timer = null;
            }

            base.OnDestroy();
        }

        #endregion

        private void SpawnSheep()
        {
            int count = SpawnManager.Instance.SpawnPointCount(GameManager.Instance.GameGameData.SheepSpawnTag);
            Debug.Log($"Spawning {count} sheep");

            for(int i = 0; i < count; ++i) {
                SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
                spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData, _sheepPen.transform);
            }

            GameManager.Instance.Reset(count);
        }

        #region Event Handlers

        protected override void GameStartServerEventHandler(object sender, EventArgs args)
        {
            base.GameStartServerEventHandler(sender, args);

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.GoalSpawnTag);
            _goal = spawnPoint.SpawnFromPrefab(GameManager.Instance.GameGameData.GoalPrefab, null) as Goal;
            _goal.SetWaypoint(_initialGoalWaypoint);

            GameManager.Instance.RoundWonEvent += RoundWonEventHandler;
        }

        protected override void GameStartClientEventHandler(object sender, EventArgs args)
        {
            base.GameStartClientEventHandler(sender, args);

            // TODO: if we have an intro, we want to get notified of it finishing here
            //GameManager.Instance.IntroCompleteEvent += IntroCompleteEventHandler;
            IntroCompleteEventHandler(null, null);
        }

        private void IntroCompleteEventHandler(object sender, EventArgs args)
        {
            //GameManager.Instance.IntroCompleteEvent -= IntroCompleteEventHandler;

            SpawnSheep();

            _timer.Start(_roundSeconds);
        }

        private void LevelTimesUpEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Times up!");

            GameManager.Instance.GameOver();
        }

        private void RoundWonEventHandler(object sender, EventArgs args)
        {
            if(!HasNextLevel) {
                Debug.Log("You win!");

                GameManager.Instance.GameOver();
                return;
            }

            TransitionLevel();
        }

        #endregion
    }
}
