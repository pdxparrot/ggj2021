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
    }
}
