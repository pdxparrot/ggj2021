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

            GameManager.Instance.Viewer.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        }
    }
}
