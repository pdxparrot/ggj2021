using UnityEngine;
using UnityEngine.Serialization;

// TODO: move to core
namespace pdxpartyparrot.ggj2021.Util
{
    public sealed class LocalRotation : MonoBehaviour
    {
        // TODO: replace the 3 vars with a Vector3

        [SerializeField]
        [FormerlySerializedAs("xRotationDegrees")]
        [FormerlySerializedAs("_xrotation")]
        private float _xRotation;

        [SerializeField]
        [FormerlySerializedAs("yRotationDegrees")]
        [FormerlySerializedAs("_yrotation")]
        private float _yRotation;

        [SerializeField]
        [FormerlySerializedAs("zRotationDegrees")]
        [FormerlySerializedAs("_zrotation")]
        private float _zRotation;

        [SerializeField]
        private float _speed = 100.0f;

        #region Unity Lifecycle

        private void Update()
        {
            float dt = Time.deltaTime;

            float xrot = _xRotation * _speed * dt;
            float yrot = _yRotation * _speed * dt;
            float zrot = _zRotation * _speed * dt;
            transform.Rotate(xrot, yrot, zrot, Space.Self);
        }

        #endregion
    }
}
