using Cinemachine;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Camera
{
    [RequireComponent(typeof(CinemachinePOV))]
    public class FollowCamera : CinemachineViewer, IPlayerViewer
    {
        public Viewer Viewer => this;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        public virtual void Initialize(GameData gameData)
        {
            Viewer.Set3D(gameData.FoV);
        }
    }
}
