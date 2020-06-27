using System;
using System.Collections;
using System.Collections.Generic;
using Mechanics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FlipCollider _flipCollider;

    [SerializeField]
    private Vector3 _flipPivotPoint;

    private Rigidbody2D _rigidbody;

    [SerializeField]
    private bool showDebug;

    [SerializeField]
    public LayerMask ground = default;

    [SerializeField]
    private Animator animator = default;

    [SerializeField]
    private Animator flippedAnimator = default;

    private float horizontalFlipScale;

    [SerializeField]
    private Color flipVisibleColor = default;

    [SerializeField]
    private float rotationAdjustmentSpeed = 5f;

    private static readonly int walking = Animator.StringToHash("Walking");

    [SerializeField]
    private float _moveSpeed;


    private float EDGE_CAST_WIDTH = .15f;
    private const float EDGE_CAST_DEPTH = .3f;
    private const float CEILING_CAST_WIDTH = .21f;

    private Vector2 _move;

    private bool previousFlippable;
    private Coroutine coroutine;
    private bool fadeRunning;

    [SerializeField]
    private Transform flipSpriteOffset = default;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        horizontalFlipScale = animator.transform.localScale.x;
        UpdateMirrorPosition();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Flip")) flip();
        Move();
        Animate(_move);
        FlipStuff();
    }

    void Move()
    {
        _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_move == Vector2.zero) return;

        Ray leftEdgeRay = new Ray(
            transform.position +
            transform.TransformDirection(new Vector3(-EDGE_CAST_WIDTH, .20f, 0)),
            transform.TransformDirection(Vector3.down * EDGE_CAST_DEPTH));
        Ray rightEdgeRay = new Ray(
            transform.position +
            transform.TransformDirection(new Vector3(EDGE_CAST_WIDTH, .20f, 0)),
            transform.TransformDirection(Vector3.down * EDGE_CAST_DEPTH));

        RaycastHit2D leftEdgeCast = Physics2D.Raycast(leftEdgeRay.origin, leftEdgeRay.direction, 1f, ground);
        RaycastHit2D rightEdgeCast = Physics2D.Raycast(rightEdgeRay.origin, rightEdgeRay.direction, 1f, ground);

        if (showDebug)
        {
            Debug.DrawRay(leftEdgeRay.origin, leftEdgeRay.direction * EDGE_CAST_DEPTH, Color.green);
            Debug.DrawRay(rightEdgeRay.origin, rightEdgeRay.direction * EDGE_CAST_DEPTH, Color.green);
        }

        Collider2D detectedGround = _move.x > 0 ? rightEdgeCast.collider : leftEdgeCast.collider;
        if (!detectedGround)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        transform.Translate(Vector3.right * (_move.x * (_moveSpeed * Time.deltaTime)), transform);
    }

    private void FlipStuff()
    {
        if (_flipCollider.CanFlip)
        {
            if (!previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeMirrorIn();
            }

            UpdateMirrorPosition();
        }
        else if (!_flipCollider.CanFlip)
        {
            if (previousFlippable)
            {
                if (fadeRunning) StopCoroutine(coroutine);
                FadeMirrorOut();
            }
        }

        previousFlippable = _flipCollider.CanFlip;
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
            animator.SetBool(walking, false);
            flippedAnimator.SetBool(walking, false);
            return;
        }


        animator.SetBool(walking, true);
        flippedAnimator.SetBool(walking, true);

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

    private void flip()
    {
        if (_flipCollider.CanFlip)
        {
            transform.RotateAround(transform.TransformPoint(_flipPivotPoint), 180);
        }
    }

    private void FadeMirrorIn()
    {
        coroutine = StartCoroutine(FadeMirror(flipVisibleColor, 1f));
    }

    private void FadeMirrorOut()
    {
        coroutine = StartCoroutine(FadeMirror(new Color(1, 1, 1, 0), 1f));
    }

    private IEnumerator FadeMirror(Color end, float time)
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