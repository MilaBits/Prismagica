using System.Collections;
using Mechanics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = default;

    [SerializeField]
    private float maxSpeed = default;

    [SerializeField]
    private Color flipVisibleColor = default;

    [SerializeField]
    private float rotationAdjustmentSpeed = 5f;

    [SerializeField]
    private Vector3 flipPivotPoint = default;

    [SerializeField]
    private float floorCheckWidth = default;

    [SerializeField]
    public LayerMask ground = default;

    [SerializeField]
    private Rigidbody rigidBody = default;

    [SerializeField]
    private Animator animator = default;

    [SerializeField]
    private Animator flippedAnimator = default;

    [SerializeField]
    private FlipCollider flipCollider = default;

    [SerializeField]
    private Transform flipSpriteOffset = default;

    private const float EDGE_CAST_DEPTH = .3f;
    private const float EDGE_CAST_WIDTH = .15f;
    private const float CEILING_CAST_WIDTH = .21f;

    [SerializeField]
    private bool showDebug = default;

    private bool previousFlippable;
    private Coroutine coroutine;
    private bool fadeRunning;

    private float horizontalFlipScale;

    private static readonly int walking = Animator.StringToHash("Walking");

    private void Start()
    {
        horizontalFlipScale = animator.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        DoTheGravityThing();
        Move();
        FlipStuff();
    }

    private void FlipStuff()
    {
        if (flipCollider.canFlip)
        {
            if (!previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeArtIn();
            }

            Ray ray = new Ray(transform.position + transform.TransformDirection(Vector3.down * .2f),
                transform.TransformDirection(Vector3.up));

            Physics.Raycast(ray.origin, ray.direction, out var hit, 1f, ground);
            flipSpriteOffset.transform.position = hit.point;

            flipSpriteOffset.rotation = Quaternion.Lerp(flipSpriteOffset.rotation,
                Quaternion.LookRotation(Vector3.forward, -hit.normal), Time.deltaTime * rotationAdjustmentSpeed);
        }
        else if (!flipCollider.canFlip)
        {
            if (previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeArtOut();
            }
        }

        previousFlippable = flipCollider.canFlip;
    }

    void Move()
    {
        var move = Input.GetAxis("Horizontal");
//        Debug.Log(Input.GetAxis("Horizontal"));

        RaycastHit leftEdgeCast;
        RaycastHit rightEdgeCast;

        Ray leftEdgeRay = new Ray(
            transform.position + transform.TransformDirection(new Vector3(-EDGE_CAST_WIDTH, .20f, 0)),
            transform.TransformDirection(Vector3.down * EDGE_CAST_DEPTH));
        Ray rightEdgeRay = new Ray(
            transform.position + transform.TransformDirection(new Vector3(EDGE_CAST_WIDTH, .20f, 0)),
            transform.TransformDirection(Vector3.down * EDGE_CAST_DEPTH));

        if (showDebug)
        {
            Debug.DrawRay(leftEdgeRay.origin, leftEdgeRay.direction * EDGE_CAST_DEPTH, Color.green);
            Debug.DrawRay(rightEdgeRay.origin, rightEdgeRay.direction * EDGE_CAST_DEPTH, Color.green);
        }

        Physics.Raycast(leftEdgeRay.origin, leftEdgeRay.direction, out leftEdgeCast, 1f, ground);
        Physics.Raycast(rightEdgeRay.origin, rightEdgeRay.direction, out rightEdgeCast, 1f, ground);


        RaycastHit leftCeilingCast;
        RaycastHit rightCeilingCast;

        Ray leftCeilingRay = new Ray(
            transform.position + transform.TransformDirection(new Vector3(0, .45f, 0)),
            transform.TransformDirection(Vector3.left * CEILING_CAST_WIDTH));
        Ray rightCeilingRay = new Ray(
            transform.position + transform.TransformDirection(new Vector3(0, .45f, 0)),
            transform.TransformDirection(Vector3.right * CEILING_CAST_WIDTH));


        if (showDebug)
        {
            Debug.DrawRay(leftCeilingRay.origin, leftCeilingRay.direction * CEILING_CAST_WIDTH, Color.red);
            Debug.DrawRay(rightCeilingRay.origin, rightCeilingRay.direction * CEILING_CAST_WIDTH, Color.red);
        }

        Physics.Raycast(leftCeilingRay.origin, leftCeilingRay.direction, out leftCeilingCast, CEILING_CAST_WIDTH,
            ground);
        Physics.Raycast(rightCeilingRay.origin, rightCeilingRay.direction, out rightCeilingCast, CEILING_CAST_WIDTH,
            ground);

        if ((!leftEdgeCast.collider || leftCeilingCast.collider) && move < 0 ||
            (!rightEdgeCast.collider || rightCeilingCast.collider) && move > 0)
        {
            move = 0;
            rigidBody.velocity = Vector3.zero;
        }

        if (move != 0)
        {
            animator.SetBool(walking, true);
            flippedAnimator.SetBool(walking, true);

            Vector3 scale = animator.transform.localScale;
            if (move > 0)
            {
                animator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
                flippedAnimator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
            }
            else
            {
                animator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
                flippedAnimator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
            }

//            rb.MovePosition(transform.position + transform.TransformDirection(new Vector3(move * moveSpeed,0,0)) * Time.deltaTime);
            rigidBody.AddRelativeForce(move * moveSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

            if (rigidBody.velocity.magnitude > maxSpeed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        else
        {
            animator.SetBool(walking, false);
            flippedAnimator.SetBool(walking, false);
        }

        if (Input.GetButtonDown("Flip")) flip();
    }

    private void flip()
    {
        if (flipCollider.canFlip)
        {
            transform.RotateAround(transform.TransformPoint(flipPivotPoint), Vector3.forward, 180);
        }
    }

    private void DoTheGravityThing()
    {
        RaycastHit downCastLeft;
        RaycastHit downCastRight;
//        RaycastHit downCastCenter;

        if (showDebug)
        {
            Debug.DrawRay(
                transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
                transform.TransformDirection(Vector3.left * floorCheckWidth),
                transform.TransformDirection(Vector3.down),
                Color.magenta);
            Debug.DrawRay(
                transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
                transform.TransformDirection(Vector3.right * floorCheckWidth),
                transform.TransformDirection(Vector3.down),
                Color.magenta);
        }

        Physics.Raycast(
            transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
            transform.TransformDirection(Vector3.left * floorCheckWidth), transform.TransformDirection(Vector3.down),
            out downCastLeft, 1, ground);
        Physics.Raycast(
            transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
            transform.TransformDirection(Vector3.right * floorCheckWidth), transform.TransformDirection(Vector3.down),
            out downCastRight, 1, ground);
//        Physics.Raycast(
//            transform.position + transform.TransformDirection(Vector3.up * 0.25f), transform.TransformDirection(Vector3.down),
//            out downCastCenter, 1, ground);


        if (downCastLeft.collider != null && downCastRight.collider != null)
        {
            Physics.gravity = ((-downCastRight.normal + -downCastLeft.normal) * 0.5f) * 9.8f;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, -Physics.gravity), Time.fixedDeltaTime * rotationAdjustmentSpeed);

//        transform.position = downCastCenter.point;

        if (showDebug) Debug.DrawRay(transform.position, Physics.gravity, Color.cyan);
    }

    private void FadeArtIn()
    {
        coroutine = StartCoroutine(FadeArt(flipVisibleColor, 1f));
    }

    private void FadeArtOut()
    {
        coroutine = StartCoroutine(FadeArt(new Color(1, 1, 1, 0), 1f));
    }

    private IEnumerator FadeArt(Color end, float time)
    {
        fadeRunning = true;
        float elapsedTime = 0;

        SpriteRenderer[] renderers = flippedAnimator.transform.GetComponentsInChildren<SpriteRenderer>();

        while (elapsedTime < time)
        {
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = Color.Lerp(renderer.color, end, (elapsedTime / time));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = end;
        }

        fadeRunning = false;
        yield return new WaitForEndOfFrame();
    }
}