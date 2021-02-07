using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.Level;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    public sealed class BlendShader : MonoBehaviour
    {
        [SerializeField]
        private string _parameter = "BlendPct";

        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        [ReadOnly]
        private float _lastPercent;

        #region Unity Lifecycle

        private void Awake()
        {
            if(null == _renderer) {
                _renderer = GetComponent<Renderer>();
            }
        }

        private void Update()
        {
            IBaseLevel level = GameManager.Instance.LevelHelper as IBaseLevel;

            foreach(Material material in _renderer.materials) {
                material.SetFloat(_parameter, level.TimePercent);
            }
            _lastPercent = level.TimePercent;
        }

        #endregion
    }
}
