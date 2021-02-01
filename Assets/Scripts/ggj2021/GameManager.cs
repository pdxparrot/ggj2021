using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2021.Camera;
using pdxpartyparrot.ggj2021.Data;
using pdxpartyparrot.ggj2021.Level;
using pdxpartyparrot.ggj2021.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2021
{
    public sealed class GameManager : GameManager<GameManager>
    {
        #region Events

        public event EventHandler<EventArgs> LevelEnterEvent;

        public event EventHandler<EventArgs> GoalScoredEvent;

        public event EventHandler<EventArgs> RoundWonEvent;

        #endregion

        public GameData GameGameData => (GameData)GameData;

        public IBaseLevel BaseLevel => (IBaseLevel)LevelHelper;

        public GameViewer Viewer { get; private set; }

        [SerializeField]
        [ReadOnly]
        private int _goal;

        public int Goal => _goal;

        [SerializeField]
        [ReadOnly]
        private int _score;

        public int Score => _score;

        public bool IsGameWon => IsGameOver && _score >= _goal;

        public bool IsGameLost => IsGameOver && _score < _goal;

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }

        public void Reset(int goal)
        {
            _goal = goal;
            _score = 0;

            GameUIManager.Instance.GameGameUI.PlayerHUD.Reset(_goal);
        }

        #region Event Handlers

        public void OnLevelEntered()
        {
            LevelEnterEvent?.Invoke(this, null);
        }

        public void OnSheepCollected()
        {
            if(null != GameUIManager.Instance.GameGameUI) {
                GameUIManager.Instance.GameGameUI.PlayerHUD.AddSlot();
            }
        }

        public void OnSheepLost()
        {
            if(null != GameUIManager.Instance.GameGameUI) {
                GameUIManager.Instance.GameGameUI.PlayerHUD.RemoveSlot();
            }
        }

        public bool OnGoalScored()
        {
            if(!IsGameReady || IsGameOver) {
                return false;
            }

            GoalScoredEvent?.Invoke(this, null);

            Debug.Log("Goooooaaaalllll!");

            _score++;

            if(null != GameUIManager.Instance.GameGameUI) {
                GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateScore(_score);
            }

            if(_goal > 0 && _score >= _goal) {
                Debug.Log("You win this round!");
                RoundWonEvent?.Invoke(this, null);
                return true;
            }

            return true;
        }

        #endregion
    }
}
