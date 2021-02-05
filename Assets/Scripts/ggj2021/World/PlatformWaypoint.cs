using pdxpartyparrot.Core.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class PlatformWaypoint : Waypoint
    {
        [SerializeField]
        private PlatformWaypoint _nextWaypoint;

        public PlatformWaypoint NextWaypoint => _nextWaypoint;

        [SerializeField]
        private float _cooldown;

        public float Cooldown => _cooldown;
    }
}
