using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2021.World;

using UnityEngine;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2021.Level
{
    public class TestSceneHelper : Game.Level.TestSceneHelper, IBaseLevel
    {
        [SerializeField]
        private Key _spawnSheepKey = Key.L;

        [SerializeField]
        private GoalWaypoint _initialGoalWaypoint;

        private GameObject _sheepPen;

        public Transform SheepPen => _sheepPen.transform;

        private Goal _goal;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _sheepPen = new GameObject("Sheep Pen");
        }

        protected override void OnDestroy()
        {
            Destroy(_sheepPen);
            Destroy(_goal.gameObject);

            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            if(Keyboard.current[_spawnSheepKey].wasPressedThisFrame) {
                SpawnSheep();
            }
        }

        #endregion

        public void SpawnSheep()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
            spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData, _sheepPen.transform);
        }

        #region Event Handlers

        protected override void GameStartServerEventHandler(object sender, EventArgs args)
        {
            base.GameStartServerEventHandler(sender, args);

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.GoalSpawnTag);
            _goal = spawnPoint.SpawnFromPrefab(GameManager.Instance.GameGameData.GoalPrefab, null) as Goal;
            _goal.SetWaypoint(_initialGoalWaypoint);
        }

        #endregion
    }
}
