using Shapes;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = default;

        [SerializeField] public LayerMask ground = default;

        [Header("References")] [SerializeField]
        private Animator animator = default;

        [SerializeField] private FlipCollider flipCollider = default;

        private Vector3 flipPivotPoint = default;

        [SerializeField] private Transform flipSpriteOffset = default;

        [SerializeField] private FlippedCharacter flippedCharacter = default;

        [Header("Move Detection")] [SerializeField]
        private float edgeCastWidth = .15f;

        [SerializeField] private float edgeCastDepth = .3f;

        [SerializeField] private float wallCastDistance = .21f;

        [SerializeField] private float wallCastHeight = .45f;

        [SerializeField] private float maxFlipDistance = .2f;

        [Header("Debug")] [SerializeField] private bool showDebug = default;

        private Disc debugDisc;

        private Vector2 _move;
        private float horizontalFlipScale;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private bool previousFlippable;
        private Coroutine coroutine;
        private bool fadeRunning;
        private Vector3 rotDampVel;

        private void Start()
        {
            debugDisc = Instantiate(new GameObject("debugDisk"), transform).AddComponent<Disc>();
            debugDisc.Radius = .01f;
            debugDisc.Color = Color.red;
            debugDisc.ThicknessSpace = ThicknessSpace.Meters;

            horizontalFlipScale = animator.transform.localScale.x;
            UpdateMirrorPosition();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Flip")) Flip();
            MoveDirection moveDirection = Move();

            Animate(moveDirection);
            flippedCharacter.Animate(moveDirection);

            UpdateMirrorPosition();
        }

        MoveDirection Move()
        {
            _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            MoveDirection direction = MoveDirection.None;
            if (_move.x != 0) direction = _move.x > 0 ? MoveDirection.Right : MoveDirection.Left;
            if (direction == MoveDirection.None) return direction;

            // Check Ground surrounding player (to prevent walking off/up steep edges
            Vector3 position = transform.position;
            Ray leftEdgeRay = new Ray(
                position +
                transform.TransformDirection(new Vector3(-edgeCastWidth, .20f, 0)),
                transform.TransformDirection(Vector3.down * edgeCastDepth));
            Ray rightEdgeRay = new Ray(
                position +
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
                position + transform.TransformDirection(new Vector2(0, wallCastHeight));
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

            if (detectedGround && !detectedWall)
            {
                transform.Translate(Vector3.right * (_move.x * (moveSpeed * Time.deltaTime)), transform);
            }

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

            return direction;
        }

        private void UpdateMirrorPosition()
        {
            // determine ground under mirror image
            Vector3 position = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(
                position + transform.TransformDirection(Vector3.down * maxFlipDistance),
                transform.TransformDirection(Vector3.up),
                1f, ground);

            Transform flipSpriteTransform = flipSpriteOffset.transform;
            flipSpriteTransform.position = hit.point;

            flipSpriteOffset.up = Vector3.SmoothDamp(
                flipSpriteOffset.up,
                flipSpriteOffset.position - (flipSpriteTransform.position + new Vector3(hit.normal.x, hit.normal.y)),
                ref rotDampVel, .1f);

            // update distance value for mirror fading
            flippedCharacter.distanceFromCharacter =
                Vector3.Distance(position, flippedCharacter.transform.position);

            if (showDebug)
            {
                Debug.DrawLine(transform.position + transform.TransformDirection(Vector3.down * maxFlipDistance),
                    hit.point, Color.yellow, Time.deltaTime);
            }
        }

        private void Animate(MoveDirection move)
        {
            animator.SetBool(Walking, move != MoveDirection.None);
            if (move == MoveDirection.None) return;

            Vector3 scale = animator.transform.localScale;
            if (move == MoveDirection.Right)
                animator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
            else
                animator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
        }

        private void Flip()
        {
            if (flipCollider.CanFlip && flippedCharacter.distanceFromCharacter < maxFlipDistance)
                transform.RotateAround(transform.TransformPoint(flipPivotPoint), Vector3.forward, 180);
        }
    }
}