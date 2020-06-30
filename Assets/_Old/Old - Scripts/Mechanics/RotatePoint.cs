using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Mechanics
{
    public class RotatePoint : PowerPoint
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

        public override void Activate()
        {
            StartTurning();
        }

        public override void Deactivate()
        {
            StopTurning();
        }

        private void Start()
        {
            if (turnMode == TurnModes.Continuous) StartTurning();
        }

        // [BoxGroup("$name/Testing"), Button]
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
                // Check if the current rotation is within snap range
                if (turnSettings.@object.eulerAngles.z < rotation.z + turnSettings.snapMargin &&
                    turnSettings.@object.eulerAngles.z > rotation.z - turnSettings.snapMargin)
                {
                    StartCoroutine(RotateInSeconds(turnSettings.@object, Quaternion.Euler(rotation), .25f));
                }
            }
        }

        // [Button, BoxGroup("$name/Testing")]
        [ContextMenu("Start Turning")]
        public void StartTurning()
        {
            turning = true;
        }

        // [Button, BoxGroup("$name/Testing")]
        [ContextMenu("Stop Turning")]
        public void StopTurning()
        {
            turning = false;

            if (turnMode != TurnModes.Single)
            {
                foreach (TurnSettings turnSettings in affectedObjects)
                {
                    SnapRotation(turnSettings);
                }
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
            public int snapMargin;

            public Vector3 NextRotation()
            {
                if (rotationIndex > rotations.Length - 1) rotationIndex = 0;
                Vector3 value = rotations[rotationIndex];
                rotationIndex++;
                return value;
            }
        }
    }
}