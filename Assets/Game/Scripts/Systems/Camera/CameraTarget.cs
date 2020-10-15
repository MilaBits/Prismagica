using System;
using UnityEngine;

namespace Systems.Camera
{
    [Serializable]
    public class CameraTarget
    {
        public Transform target;
        public float zoomLevel;
        public int priority;
        public Vector3 offset = new Vector3(0, 0, -10);
        public bool RotateWithPlayer;
        public bool Interpolate;
    }
}