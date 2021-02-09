using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pdxpartyparrot
{
    public class localRotation : MonoBehaviour
    {
        [SerializeField]
        private float
        xRotationDegrees = 0.0f,
        yRotationDegrees = 0.0f,
        zRotationDegrees = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float xCurrentRotation = xRotationDegrees * Time.deltaTime;
            float yCurrentRotation = yRotationDegrees * Time.deltaTime;
            float zCurrentRotation = zRotationDegrees * Time.deltaTime;
            transform.Rotate(xRotationDegrees, yRotationDegrees, zRotationDegrees, Space.Self);
        }
    }
}
