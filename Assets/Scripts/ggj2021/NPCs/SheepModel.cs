using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.NPCs
{
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class SheepModel : MonoBehaviour
    {
        private SpineSkinHelper _skinHelper;

        public SpineSkinHelper SkinHelper => _skinHelper;

        #region Unity Lifecycle

        private void Awake()
        {
            _skinHelper = GetComponent<SpineSkinHelper>();
        }

        #endregion
    }
}
