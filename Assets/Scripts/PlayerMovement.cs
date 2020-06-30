using System;
using System.Collections;
using Mechanics;
using Shapes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    public LayerMask ground = default;

    [SerializeField]
    private Color flipVisibleColor = default;

    [Header("References")]
    [SerializeField]
    private Animator animator = default;

    [SerializeField]
    private Animator flippedAnimator = default;

    [SerializeField]
    private FlipCollider flipCollider;

    [SerializeField]
    private Vector3 flipPivotPoint;

    [SerializeField]
    private Transform flipSpriteOffset = default;

    [Header("Move Detection")]
    [SerializeField]
    private float edgeCastWidth = .15f;

    [SerializeField]
    private float edgeCastDepth = .3f;

    [SerializeField]
    private float wallCastDistance = .21f;

    [SerializeField]
    private float wallCastHeight = .45f;

    [Header("Debug")]
    [SerializeField]
    private bool showDebug;

    [SerializeField]
    private Disc debugDisc;


    private Vector2 _move;
    private Rigidbody2D _rigidbody;
    private float horizontalFlipScale;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private bool previousFlippable;
    private Coroutine coroutine;
    private bool fadeRunning;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        horizontalFlipScale = animator.transform.localScale.x;
        UpdateMirrorPosition();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Flip")) Flip();
        Move();
        Animate(_move);
        VisualizeMirrorImage();
    }

    void Move()
    {
        _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        MoveDirection direction = MoveDirection.None;
        if (_move.x != 0) direction = _move.x > 0 ? MoveDirection.Right : MoveDirection.Left;
        if (direction == MoveDirection.None) return;

        // Check Ground surrounding player (to prevent walking off/up steep edges
        Ray leftEdgeRay = new Ray(
            transform.position +
            transform.TransformDirection(new Vector3(-edgeCastWidth, .20f, 0)),
            transform.TransformDirection(Vector3.down * edgeCastDepth));
        Ray rightEdgeRay = new Ray(
            transform.position +
            transform.TransformDirection(new Vector3(edgeCastWidth, .20f, 0)),
            transform.TransformDirection(Vector3.down * edgeCastDepth));

        RaycastHit2D leftEdge = Physics2D.Raycast(
            leftEdgeRay.origin,
            leftEdgeRay.direction,
            1f, ground);
        RaycastHit2D rightEdge = Physics2D.Raycast(
            rightEdgeRay.origin,
            rightEdgeRay.direction,
            1f, ground);

        //Check Wall near player (to prevent clipping through Wall)
        Vector3 wallCastOrigin =
            transform.position + transform.TransformDirection(new Vector2(0, wallCastHeight));
        Ray wallRayLeft = new Ray(
            wallCastOrigin,
            transform.TransformDirection(Vector3.left) * wallCastDistance);
        Ray wallRayRight = new Ray(
            wallCastOrigin,
            transform.TransformDirection(Vector3.right) * wallCastDistance);

        RaycastHit2D leftWall = Physics2D.Raycast(
            wallRayLeft.origin,
            wallRayLeft.direction,
            wallCastDistance, ground);
        RaycastHit2D rightWall = Physics2D.Raycast(
            wallRayRight.origin,
            wallRayRight.direction,
            wallCastDistance, ground);


        Collider2D detectedGround = new Collider2D();
        Collider2D detectedWall = new Collider2D();
        switch (direction)
        {
            case MoveDirection.Left:
                detectedGround = leftEdge.collider;
                detectedWall = leftWall.collider;

                break;
            case MoveDirection.Right:
                detectedGround = rightEdge.collider;
                detectedWall = rightWall.collider;
                break;
        }

        if (!detectedGround || detectedWall)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        transform.Translate(Vector3.right * (_move.x * (moveSpeed * Time.deltaTime)), transform);

        if (showDebug)
        {
            if (direction == MoveDirection.Left)
            {
                Debug.DrawRay(leftEdgeRay.origin, leftEdgeRay.direction * edgeCastDepth, Color.green);
                Debug.DrawRay(wallRayLeft.origin, wallRayLeft.direction * wallCastDistance, Color.magenta);
                debugDisc.transform.position = leftWall.point;
            }

            if (direction == MoveDirection.Right)
            {
                Debug.DrawRay(rightEdgeRay.origin, rightEdgeRay.direction * edgeCastDepth, Color.green);
                Debug.DrawRay(wallRayRight.origin, wallRayRight.direction * wallCastDistance, Color.magenta);
                debugDisc.transform.position = rightWall.point;
            }
        }
    }

    private void VisualizeMirrorImage()
    {
        if (flipCollider.CanFlip)
        {
            if (!previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeMirrorIn();
            }

            UpdateMirrorPosition();
        }
        else if (!flipCollider.CanFlip)
        {
            if (previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeMirrorOut();
            }
        }

        previousFlippable = flipCollider.CanFlip;
    }

    private void UpdateMirrorPosition()
    {
        // determine ground under mirror image
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + transform.TransformDirection(Vector3.down * .5f),
            transform.TransformDirection(Vector3.up),
            1f, ground);

        Debug.DrawLine(transform.position + transform.TransformDirection(Vector3.down * .5f),
            hit.point, Color.yellow, Time.deltaTime);

        flipSpriteOffset.transform.position = transform.position + transform.InverseTransformPoint(hit.point);

        // rotate mirror image to stand on ground
        flipSpriteOffset.up = flipSpriteOffset.position -
                              (flipSpriteOffset.transform.position + new Vector3(hit.normal.x, hit.normal.y));
    }

    private void Animate(Vector2 move)
    {
        if (move.x == 0)
        {
            animator.SetBool(Walking, false);
            flippedAnimator.SetBool(Walking, false);
            return;
        }


        animator.SetBool(Walking, true);
        flippedAnimator.SetBool(Walking, true);

        Vector3 scale = animator.transform.localScale;
        if (move.x > 0)
        {
            animator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
            flippedAnimator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
        }
        else
        {
            animator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
            flippedAnimator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
        }
    }

    private void Flip()
    {
        if (flipCollider.CanFlip)
        {
            transform.RotateAround(transform.TransformPoint(flipPivotPoint), Vector3.forward, 180);
        }
    }

    private void FadeMirrorIn() => coroutine = StartCoroutine(FadeMirror(flipVisibleColor, 1f));

    private void FadeMirrorOut() => coroutine = StartCoroutine(FadeMirror(new Color(1, 1, 1, 0), 1f));

    private IEnumerator FadeMirror(Color end, float time)
    {
        fadeRunning = true;
        float elapsedTime = 0;

        SpriteRenderer[] bodyPartRenderers = flippedAnimator.transform.GetComponentsInChildren<SpriteRenderer>();

        while (elapsedTime < time)
        {
            foreach (SpriteRenderer bodyPartRenderer in bodyPartRenderers)
            {
                bodyPartRenderer.color = Color.Lerp(bodyPartRenderer.color, end, (elapsedTime / time));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (SpriteRenderer bodyPartRenderer in bodyPartRenderers)
        {
            bodyPartRenderer.color = end;
        }

        fadeRunning = false;
        yield return new WaitForEndOfFrame();
    }

    private enum MoveDirection
    {
        None,
        Left,
        Right
    }
}