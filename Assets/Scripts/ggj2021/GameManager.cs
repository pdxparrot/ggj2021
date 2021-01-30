using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2021.Camera;
using pdxpartyparrot.ggj2021.Data;
using pdxpartyparrot.ggj2021.Level;

using UnityEngine;

namespace pdxpartyparrot.ggj2021
{
    public sealed class GameManager : GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public IBaseLevel BaseLevel => (IBaseLevel)LevelHelper;

        public GameViewer Viewer { get; private set; }

        [SerializeField]
        [ReadOnly]
        private int _goal;

        public int Goal
        {
            get => _goal;
            set => _goal = value;
        }

        [SerializeField]
        [ReadOnly]
        private int _score;

        public int Score => _score;

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }

        #region Event Handlers

        public void OnGoalScored()
        {
            Debug.Log("Goooooaaaalllll!");

            _score++;
            if(_goal > 0 && _score >= _goal) {
                Debug.Log("You win!");

                GameOver();
            }
        }

        #endregion
    }
}
