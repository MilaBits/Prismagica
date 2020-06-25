using System.Collections.Generic;
using Enums;
using Mechanics;
using UnityEngine;

namespace UI
{
    public class RadialMenu : MonoBehaviour
    {
        public Power power;

        [SerializeField]
        private Pixi Pixi = null;

        [SerializeField]
        private PowerInteractor powerInteractor = null;

        [SerializeField]
        private Camera GameCamera = null;

        [SerializeField]
        private Transform center = null;

        [SerializeField]
        private Transform menu = null;

        [SerializeField]
        private Dictionary<Power, GameObject> lights = new Dictionary<Power, GameObject>();

        private Vector3 direction;
        private float angle;

        private void Start()
        {
            menu.gameObject.SetActive(false);
        }

        void UpdateDirection()
        {
            direction = transform.InverseTransformDirection(GameCamera.ScreenToWorldPoint(Input.mousePosition) -
                                                            center.position);
            direction += Vector3.forward * 10;
            Debug.DrawRay(center.position, direction, Color.green);
            angle = (Mathf.Atan2(direction.x, direction.y) * 180 / Mathf.PI);
        }

        void Update()
        {
            UpdateDirection();

            if (Input.GetButtonDown("Power Wheel"))
            {
                transform.position = Pixi.transform.position;
                menu.gameObject.SetActive(true);
            }

            if (Input.GetButtonUp("Power Wheel"))
            {
                Debug.Log(angle + ": " + power);
                menu.gameObject.SetActive(false);

//            Pixi.PixiFlight.MoveMode = power == Power.Black ? MoveMode.Click : MoveMode.Follow;
                return;
            }

            if (menu.gameObject.activeSelf)
            {
                foreach (KeyValuePair<Power, GameObject> pair in lights)
                {
                    pair.Value.SetActive(false);
                }

                if (direction.magnitude < .3f)
                {
                    power = Power.Black;
                }
                else if (angle > -36 && angle < 36)
                {
                    power = Power.Yellow;
                }
                else if (angle > 36 && angle < 108)
                {
                    power = Power.Green;
                }
                else if (angle > 36 && angle < 108)
                {
                    power = Power.Blue;
                }
                else if (angle > 108 && angle < 180)
                {
                    power = Power.Blue;
                }
                else if (angle > -108 && angle < -36)
                {
                    power = Power.Red;
                }
                else if (angle > -180 && angle < 108)
                {
                    power = Power.Purple;
                }
            }
        }
    }
}