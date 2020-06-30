using UnityEngine;

namespace Mechanics
{
    public class Pixi : MonoBehaviour
    {
        [SerializeField]
        private float energy = 100;

        [SerializeField]
        private PowerInteractor powerInteractor = null;

        [SerializeField]
        private GameObject glow = null;

        public PixiFlight pixiFlight = null;

        public PowerPoint lastPowerPoint;

        public bool powerActive;
    }
}