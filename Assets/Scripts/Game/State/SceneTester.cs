using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    // TODO: this needs to be abstract because we need to deal with viewer configuration
    public abstract class SceneTester : MainGameState
    {
        [SerializeField]
        private string[] _testScenes;

        public string[] TestScenes => _testScenes;

        protected override bool InitializeClient()
        {
            // need to init the viewer before we start spawning players
            // so that they have a viewer to attach to
            InitViewer();

            if(!base.InitializeClient()) {
                Debug.LogWarning("Failed to initialize client!");
                return false;
            }

            return true;
        }

        public abstract void InitViewer();

        public void SetScene(string sceneName)
        {
            CurrentSceneName = sceneName;
        }
    }
}
