using System.Linq;
using Enums;
using UnityEngine;

namespace Mechanics
{
    public class PowerInteractor : MonoBehaviour
    {
        [SerializeField]
        private LayerMask powerLayer = default;

        [SerializeField]
        private Power[] powers = default;

        private void Update()
        {
            if (Input.GetButtonDown("Power"))
            {
                PowerPoint powerPoint = GetPowerPoint();
                if (powerPoint != null && powers.Any(x => x == powerPoint.power)) powerPoint.Activate();
            }
        }

        private PowerPoint GetPowerPoint()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + Vector3.back, Vector3.forward, out hit, 100f, powerLayer);


            if (!hit.transform || !hit.transform.GetComponent<PowerPoint>()) return null;
            PowerPoint powerPoint = hit.transform.GetComponent<PowerPoint>();
            return powerPoint;
        }
    }
}