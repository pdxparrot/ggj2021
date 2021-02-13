using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2021.Level;

using UnityEngine;

namespace pdxpartyparrot.ggj2021.World
{
    // TODO: would be really cool to figure out a way to make this core
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
            _lastPercent = level.TimePercent;

            foreach(Material material in _renderer.materials) {
                material.SetFloat(_parameter, _lastPercent);
            }
        }

        #endregion
    }
}
