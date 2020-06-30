using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Attackable : MonoBehaviour
    {
        private Directions acceptingDirections;

        private Vector3 attackEntry;
        private Vector3 rawAttackDirection;

        private bool movable = false;

        private float moveSpeed = 1;

        private Dictionary<Directions, Vector3> directions = new Dictionary<Directions, Vector3>()
        {
            {Directions.Up, Vector3.up},
            {Directions.Right, Vector3.right},
            {Directions.Down, Vector3.down},
            {Directions.Left, Vector3.left}
        };

        public UnityEvent onHit;

        private Rigidbody rb;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("entered");
            Pixi pixi = other.gameObject.GetComponentInParent<Pixi>();
            if (pixi && pixi.powerActive)
            {
                PixiFlight flight = pixi.GetComponent<PixiFlight>();

                if (movable)
                {
                    AddVelocity(flight.GetDirection(), moveSpeed);
                    flight.InterruptMove();
                }

                pixi.powerActive = false;
            }
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (rb)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, rb.velocity.magnitude - rb.drag * Time.deltaTime);
            }
        }

        public void AddVelocity(Vector3 direction, float speed)
        {
            rb.velocity = direction.normalized * speed;
            Debug.Log(rb.velocity);
        }

        static IEnumerable<Directions> GetFlags(Directions input)
        {
            foreach (Directions value in Directions.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}