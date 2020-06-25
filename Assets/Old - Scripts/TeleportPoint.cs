    using System;
    using UnityEngine;

    [Serializable]
    public struct TeleportPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public TeleportPoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
