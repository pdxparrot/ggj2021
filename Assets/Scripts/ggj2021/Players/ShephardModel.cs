using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.Players
{
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class ShephardModel : MonoBehaviour
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
