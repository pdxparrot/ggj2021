using UnityEngine;

namespace pdxpartyparrot.Core.World
{
    public class Waypoint : MonoBehaviour
    {
        #region Unity Lifecycle

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1);
        }

        #endregion
    }
}
