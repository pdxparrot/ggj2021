using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.State
{
    public class SceneTester : Game.State.SceneTester
    {
        public override void InitViewer()
        {
            ViewerManager.Instance.AllocateViewers(1, GameManager.Instance.GameGameData.GameViewerPrefab);
            GameManager.Instance.InitViewer();
        }
    }
}
