using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.UI
{
    public sealed class CompassNeedle : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private Goal _goal;

        public void Initialize(Goal goal)
        {
            _goal = goal;
        }

        public void UpdateAngle(Vector3 playerPosition)
        {
            Vector3 d = _goal.transform.position - playerPosition;
            float angle = Vector3.SignedAngle(Vector3.forward, d, Vector3.up);

            Vector3 rot = transform.eulerAngles;
            rot.z = -angle;
            transform.eulerAngles = rot;
        }
    }
}
