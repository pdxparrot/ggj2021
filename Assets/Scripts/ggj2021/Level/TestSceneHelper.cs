using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2021.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2021.Level
{
    public class TestSceneHelper : Game.Level.TestSceneHelper, IBaseLevel
    {
        [SerializeField]
        private Key _spawnSheepKey = Key.L;

        [SerializeField]
        private GoalWaypoint _initialGoalWaypoint;

        [SerializeField]
        private float _goalSpeed = 5.0f;

        [CanBeNull]
        private GameObject _sheepPen;

        [CanBeNull]
        public Transform SheepPen => null == _sheepPen ? null : _sheepPen.transform;

        private Goal _goal;

        #region Unity Lifecycle

        private void FixedUpdate()
        {
            if(Keyboard.current[_spawnSheepKey].wasPressedThisFrame) {
                SpawnSheep();
            }
        }

        #endregion

        private void SpawnSheep()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
            spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData, _sheepPen.transform);
        }

        private void SpawnGoal()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.GoalSpawnTag);
            _goal = spawnPoint.SpawnFromPrefab(GameManager.Instance.GameGameData.GoalPrefab, null) as Goal;
            _goal.Initialize(_initialGoalWaypoint, _goalSpeed);
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_sheepPen);
            _sheepPen = new GameObject("Sheep Pen");

            SpawnGoal();
        }

        protected override void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            // TODO: this would be better if it was behind
            // some new event that fires after the loading screen is up

            Destroy(_sheepPen);
            _sheepPen = null;

            Destroy(_goal.gameObject);
            _goal = null;

            base.GameUnReadyEventHandler(sender, args);
        }

        #endregion
    }
}
