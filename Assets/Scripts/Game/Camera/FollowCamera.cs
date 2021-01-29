using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Camera
{
    // TODO: is this even useful? it's entirely reliant on selecting a Body that follows something

    public class FollowCamera : CinemachineViewer, IPlayerViewer
    {
        public Viewer Viewer => this;

        public virtual void Initialize(GameData gameData)
        {
            Viewer.Set3D(gameData.FoV);
        }

        public void FollowTarget(GameObject target)
        {
            LookAt(target.transform);
            Follow(target.transform);
        }
    }
}
