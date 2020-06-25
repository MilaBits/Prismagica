using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    [RequireComponent(typeof(LineRenderer))]
    public class ConnectPoint : PowerPoint
    {
        public bool source;

        private List<ConnectPoint> connections = new List<ConnectPoint>();

        private LineRenderer lineRenderer;

        public UnityEvent poweredEvent;

        private bool tether;

        [SerializeField]
        private bool end = default;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void SetLineEnd(Vector3 position)
        {
            tether = false;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[] {transform.position, position});
        }

        public override void Activate()
        {
            if (!end) tether = true;

            if (pixi.lastPowerPoint is ConnectPoint)
            {
                ConnectPoint lastPoint = (ConnectPoint) pixi.lastPowerPoint;

                // if the last point wasn't a source we do nothing but update the last point
                if (!lastPoint.source)
                {
                    pixi.lastPowerPoint = this;
                    return;
                }

                if (!source)
                {
                    connections.Add(lastPoint);

                    source = true;
                    poweredEvent.Invoke();

                    lastPoint.SetLineEnd(transform.position);
//            StartCoroutine(DrawLine(lastPoint.transform.position, transform.position, .5f));
                }
            }

            pixi.lastPowerPoint = this;
        }

        private void Update()
        {
            if (tether)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(new[] {transform.position, pixi.transform.position});
            }
        }

        private IEnumerator DrawLine(Vector3 start, Vector3 end, float duration)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new[] {transform.position, transform.position});
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                lineRenderer.SetPosition(1, Vector3.Lerp(start, end, t));
                yield return null;
            }

            lineRenderer.SetPosition(1, end);
        }

        public override void Deactivate()
        {
        }
    }
}