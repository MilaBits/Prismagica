using UnityEngine;

public class GravityThing : MonoBehaviour
{
    [SerializeField]
    private bool showDebug;

    [SerializeField]
    public LayerMask ground = default;

    [SerializeField]
    private float floorCheckWidth = default;

    [SerializeField]
    private float rotationAdjustmentSpeed = 5f;

    private Vector2 gravity;

    [SerializeField]
    private float gravityPower = 9.8f;

    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() => DoTheGravityThing();

    private void DoTheGravityThing()
    {
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

        RaycastHit2D groundCastLeft = Physics2D.Raycast(
            transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
            transform.TransformDirection(Vector3.left * floorCheckWidth),
            transform.TransformDirection(Vector3.down),
            1f, ground);
        RaycastHit2D groundCastRight = Physics2D.Raycast(
            transform.position + transform.TransformDirection(Vector3.up * 0.25f) +
            transform.TransformDirection(Vector3.right * floorCheckWidth),
            transform.TransformDirection(Vector3.down),
            1f, ground);

        if (groundCastLeft.collider != null && groundCastRight.collider != null)
        {
            gravity = ((-groundCastRight.normal + -groundCastLeft.normal) * 0.5f) * gravityPower;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, -gravity),
            Time.fixedDeltaTime * rotationAdjustmentSpeed);

        _rigidBody2D.AddForce(gravity);

        if (showDebug) Debug.DrawRay(transform.position, gravity, Color.cyan);
    }
}