using System;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Level;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Level
{
    public sealed class MainLevel : LevelHelper
    {
        [SerializeField]
        private float _roundSeconds = 60.0f;

        [SerializeReference]
        [ReadOnly]
        private ITimer _timer;

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

        #endregion

        #region Event Handlers

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

            // TODO: we want to spawn the sheep or whatever here

            _timer.Start(_roundSeconds);
        }

        private void LevelTimesUpEventHandler(object sender, EventArgs args)
        {
            Debug.Log("Times up!");

            GameManager.Instance.GameOver();
        }

        #endregion
    }
}
