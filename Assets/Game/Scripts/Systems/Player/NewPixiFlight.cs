using System.Collections;
using System.Linq;
using Enums;
using Flask;
using Shapes;
using UnityEngine;

namespace Systems.Player
{
    public class NewPixiFlight : MonoBehaviour
    {
        private MoveMode moveMode = MoveMode.Follow;

        private bool following;
        private Vector2 dampVelocity;
        private UnityEngine.Camera cam;

        [SerializeField] private float launchMultiplier = 2;

        [SerializeField] private AnimationCurve launchCurve = default;

        private Vector3 launchPivot;
        private bool held;
        private float heldDuration;

        private Vector3 launchTarget;
        private bool drawLaunch;

        private Line targetLine;
        private Line pullLine;
        private Disc pivotDisc;

        private DTweenVector2 _position;
        [SerializeField]
        private float followStrength = 5;

        void Start()
        {
            cam = UnityEngine.Camera.main;

            pivotDisc = Instantiate(new GameObject("Shapes Disc")).AddComponent<Disc>();
            pullLine = Instantiate(new GameObject("Shapes Line")).AddComponent<Line>();
            targetLine = Instantiate(new GameObject("Shapes Line")).AddComponent<Line>();

            targetLine.Geometry = LineGeometry.Billboard;
            pullLine.Geometry = LineGeometry.Billboard;

            targetLine.ThicknessSpace = ThicknessSpace.Meters;
            pullLine.ThicknessSpace = ThicknessSpace.Meters;
            pivotDisc.ThicknessSpace = ThicknessSpace.Meters;

            targetLine.Dashed = true;
            pullLine.Dashed = true;

            targetLine.DashSize = 2;
            pullLine.DashSize = 2;

            targetLine.Thickness = .05f;
            pullLine.Thickness = .05f;
            pivotDisc.Radius = .05f;


            targetLine.Color = Color.white;
            pullLine.Color = Color.gray;
            pivotDisc.Color = Color.white;

            _position = new DTweenVector2(transform.position, followStrength);
            
            transform.SetParent(null);
        }

        void Update()
        {
            Vector3 target = MouseToFlyTarget();

            if (Input.GetButtonDown("Power"))
            {
                held = true;
            }
            
            if (Input.GetButtonUp("Power"))
            {
                held = false;
                heldDuration = 0;
            }

            if (held) heldDuration += Time.deltaTime;

            if (heldDuration > .05f && moveMode != MoveMode.Launch)
            {
                StartCoroutine(Launch(target));
            }
            

            if (moveMode != MoveMode.None) Follow(target);
            Draw();
        }

        private Vector3 MouseToFlyTarget()
        {
            Vector3 target = cam.ScreenToWorldPoint(Input.mousePosition);
            target = new Vector2(target.x, target.y);
            return target;
        }

        private IEnumerator Launch(Vector3 mousePos)
        {
            moveMode = MoveMode.Launch;
            launchPivot = mousePos;

            // Calculate target position
            drawLaunch = true;
            while (!Input.GetButtonUp("Power"))
            {
                Vector3 direction = launchPivot - transform.position;
                launchTarget = launchPivot + direction * launchMultiplier;

                yield return null;
            }

            drawLaunch = false;

            // Launch towards target position
            moveMode = MoveMode.None;
            Vector3 start = transform.position;
            float duration = launchCurve.keys.Last().time;
            for (float timePassed = 0; timePassed < duration; timePassed += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(start, launchTarget, launchCurve.Evaluate(timePassed / duration));
                yield return null;
            }

            // Return to normal
            yield return new WaitForSeconds(.2f);
            _position = new DTweenVector2(transform.position, followStrength);
            moveMode = MoveMode.Follow;
        }

        private void Follow(Vector3 mousePos)
        {
            
            _position.omega = followStrength;
            _position.Step(mousePos);
            transform.position = _position.position;
            
            // transform.position = Vector2.SmoothDamp(transform.position, mousePos, ref dampVelocity, .3f);
        }

        private void Draw()
        {
            if (drawLaunch)
            {
                pullLine.enabled = true;
                targetLine.enabled = true;
                pivotDisc.enabled = true;

                pullLine.Start = (launchPivot);
                pullLine.End = (transform.position);

                targetLine.Start = (launchTarget);
                targetLine.End = (launchPivot);

                pivotDisc.transform.position = launchPivot;
            }
            else
            {
                pullLine.enabled = false;
                targetLine.enabled = false;
                pivotDisc.enabled = false;
            }
        }
    }
}