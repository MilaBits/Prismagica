using Enums;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(Collider))]
    public abstract class PowerPoint : MonoBehaviour
    {
        [SerializeField]
        internal Power power = default;

        internal Pixi pixi;

        public abstract void Activate();

        public abstract void Deactivate();
    }
}