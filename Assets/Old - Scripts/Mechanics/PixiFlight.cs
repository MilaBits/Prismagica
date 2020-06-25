using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanics
{
    public class PixiFlight : MonoBehaviour
    {
        [SerializeField]
        private float flightTime = .5f;

        [FormerlySerializedAs("normalFlight")]
        [SerializeField]
        public AnimationCurve normalCurve = default;

        [SerializeField]
        public AnimationCurve punchCurve = default;

        [SerializeField]
        private Transform returnTarget = default;

        private Coroutine flightCoroutine;
        private bool running;

        private Vector3 lastPos;

        private Vector3 moveStart;

        public MoveMode moveMode;

        private Vector3 dampVelocity = Vector3.zero;

        private Vector3 direction;

        public float GetFlightTime()
        {
            return flightTime;
        }

        private void Start()
        {
            transform.position = returnTarget.position;
        }

        private void Update()
        {
            // Move back to player when out of screen
            if (IsInView())
            {
                if (running) StopCoroutine(flightCoroutine);
                flightCoroutine =
                    StartCoroutine(MoveTo(
                        new Vector3(returnTarget.position.x, returnTarget.position.y, returnTarget.position.z),
                        normalCurve,
                        flightTime));
            }

            switch (moveMode)
            {
                case MoveMode.Click:
                    if (Input.GetButtonDown("Power"))
                    {
                        if (running) StopCoroutine(flightCoroutine);
                        flightCoroutine = StartCoroutine(MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                            normalCurve,
                            flightTime));
                    }

                    break;
                case MoveMode.Follow:

                    if (running) StopCoroutine(flightCoroutine);

                    var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target = new Vector3(target.x, target.y, 0);
                    transform.position = Vector3.SmoothDamp(transform.position, target, ref dampVelocity, .3f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            transform.rotation = Camera.main.transform.rotation;

            direction = (transform.position - lastPos).normalized;
            lastPos = transform.position;
        }

        public Vector3 GetDirection()
        {
            return direction;
        }

        public void InterruptMove()
        {
            if (running) StopCoroutine(flightCoroutine);
            flightCoroutine = StartCoroutine(MoveTo(moveStart, normalCurve, flightTime));
        }

        private bool IsInView()
        {
            Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
            if (screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1)
            {
                return false;
            }

            return true;
        }

        public void FlyTo(Vector3 target, AnimationCurve curve, float time)
        {
            if (running) StopCoroutine(flightCoroutine);
            flightCoroutine = StartCoroutine(MoveTo(target, curve, time));
        }

        private IEnumerator MoveTo(Vector3 target, AnimationCurve curve, float time)
        {
            running = true;
            moveStart = transform.position;

            float elapsedTime = 0;
            target = new Vector3(target.x, target.y, 0);

            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float percent = Mathf.Clamp01(elapsedTime / time);
                float curvePercent = curve.Evaluate(percent);

                transform.position = Vector3.LerpUnclamped(transform.position, target, curvePercent);

                yield return null;
            }

            transform.position = target;
            running = false;

            yield return new WaitForEndOfFrame();
        }
    }
}