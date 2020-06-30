using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Mechanics
{
    public class Turnable : MonoBehaviour
    {
        [SerializeField]
        private TurnModes turnMode = default;

        [SerializeField]
        private List<TurnSettings> affectedObjects = default;

        private bool turning;
        private Coroutine coroutine;
        private bool runningCoroutine;

        [SerializeField]
        private float freezeDuration = default;

        private void Start()
        {
            if (turnMode == TurnModes.Continuous)
            {
                StartTurning();
            }
        }

        [ContextMenu("Freeze")]
        public void Freeze()
        {
            StartCoroutine(FreezeForSeconds());
        }

        public IEnumerator FreezeForSeconds()
        {
            StopTurning();
            yield return new WaitForSeconds(freezeDuration);
            StartTurning();
        }

        private void Update()
        {
            if (turning)
            {
                if (turnMode == TurnModes.Single)
                {
                    foreach (TurnSettings turnSettings in affectedObjects)
                    {
                        if (runningCoroutine) StopCoroutine(coroutine);
                        coroutine = StartCoroutine(RotateInSeconds(
                            turnSettings.@object,
                            Quaternion.Euler(turnSettings.NextRotation()),
                            turnSettings.turnSpeed));
                    }

                    turning = false;
                    return;
                }

                foreach (TurnSettings turnSettings in affectedObjects)
                {
                    turnSettings.@object.eulerAngles += new Vector3(0, 0, turnSettings.angle * Time.deltaTime);
                }
            }
        }

        public void TurnToNext()
        {
            foreach (TurnSettings turnSettings in affectedObjects)
            {
                if (runningCoroutine) StopCoroutine(coroutine);
                coroutine = StartCoroutine(RotateInSeconds(
                    turnSettings.@object,
                    Quaternion.Euler(turnSettings.NextRotation()),
                    turnSettings.turnSpeed));
            }
        }

        private void SnapRotation(TurnSettings turnSettings)
        {
            foreach (var rotation in turnSettings.rotations)
            {
                // 70
                if (turnSettings.@object.eulerAngles.z < rotation.z + turnSettings.snapMargin && // 70 + 5 = 75
                    turnSettings.@object.eulerAngles.z > rotation.z - turnSettings.snapMargin) // 70 - 5 = 65
                {
                    StartCoroutine(RotateInSeconds(turnSettings.@object, Quaternion.Euler(rotation), .25f));
                }
            }
        }

        [ContextMenu("Start Turning")]
        public void StartTurning()
        {
            turning = true;
            Debug.Log(gameObject.name + " started turning");
        }

        [ContextMenu("Stop Turning")]
        public void StopTurning()
        {
            turning = false;
            Debug.Log(gameObject.name + " stopped turning");
            foreach (TurnSettings turnSettings in affectedObjects)
            {
                SnapRotation(turnSettings);
            }
        }

        public void StopTurningAfterSeconds(float time)
        {
            StartCoroutine(StopAfterSeconds(time));
        }

        private IEnumerator StopAfterSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            StopTurning();
        }

        private IEnumerator RotateInSeconds(Transform targetObject, Quaternion rotation, float time)
        {
            runningCoroutine = true;
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                targetObject.rotation = Quaternion.Lerp(targetObject.rotation, rotation, (elapsedTime / time));
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            targetObject.rotation = rotation;

            runningCoroutine = false;
            yield return new WaitForEndOfFrame();
        }


        [Serializable]
        private class TurnSettings
        {
            public Transform @object = default;
            public float turnSpeed = default;
            public Vector3[] rotations = default;
            public int rotationIndex = default;
            public float angle = default;
            public int snapMargin = default;

            public Vector3 NextRotation()
            {
                if (rotationIndex > rotations.Length - 1) rotationIndex = 0;
                Vector3 value = rotations[rotationIndex];
                Debug.Log(value);
                rotationIndex++;
                return value;
            }
        }
    }
}