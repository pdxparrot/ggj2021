using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.ggj2021.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.State
{
    public sealed class MainGameState : Game.State.MainGameState
    {
        protected override bool InitializeClient()
        {
            // need to init the viewer before we start spawning players
            // so that they have a viewer to attach to
            ViewerManager.Instance.AllocateViewers(1, GameManager.Instance.GameGameData.GameViewerPrefab);
            GameManager.Instance.InitViewer();

            // need this before players spawn
            GameUIManager.Instance.InitializeGameUI(GameManager.Instance.Viewer.UICamera);

            if(!base.InitializeClient()) {
                Debug.LogWarning("Failed to initialize client!");
                return false;
            }

            return true;
        }
    }
}
