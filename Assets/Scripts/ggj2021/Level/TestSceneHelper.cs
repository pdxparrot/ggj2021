using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2021.UI;

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

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _timePercent;

        public float TimePercent => _timePercent;

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

        protected override void GameStartClientEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.Reset(0);

            base.GameStartClientEventHandler(sender, args);
        }

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_sheepPen);
            _sheepPen = new GameObject("Sheep");

            GameUIManager.Instance.GameGameUI.Hide();
            GameUIManager.Instance.GameGameUI.ShowHUD(true);

            GameManager.Instance.OnLevelEntered();
        }

        protected override void LevelTransitioningEventHandler(object sender, EventArgs args)
        {
            if(null != _sheepPen) {
                Destroy(_sheepPen);
                _sheepPen = null;
            }

            base.LevelTransitioningEventHandler(sender, args);
        }

        #endregion
    }
}
