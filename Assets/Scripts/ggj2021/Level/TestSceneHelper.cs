using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.World;

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
        private Key _scoreKey = Key.G;

        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _sheepPen;

        [CanBeNull]
        public Transform SheepPen => null == _sheepPen ? null : _sheepPen.transform;

        #region Unity Lifecycle

        private void FixedUpdate()
        {
            if(Keyboard.current[_spawnSheepKey].wasPressedThisFrame) {
                SpawnSheep();
            }

            if(Keyboard.current[_scoreKey].wasPressedThisFrame) {
                GameManager.Instance.OnGoalScored();
            }
        }

        #endregion

        private void SpawnSheep()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.SheepSpawnTag);
            spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SheepPrefab, GameManager.Instance.GameGameData.SheepBehaviorData, _sheepPen.transform);
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_sheepPen);
            _sheepPen = new GameObject("Sheep");

            GameManager.Instance.OnLevelEntered();
        }

        protected override void GameUnReadyEventHandler(object sender, EventArgs args)
        {
            // TODO: this would be better if it was behind
            // some new event that fires after the loading screen is up

            Destroy(_sheepPen);
            _sheepPen = null;

            base.GameUnReadyEventHandler(sender, args);
        }

        #endregion
    }
}
