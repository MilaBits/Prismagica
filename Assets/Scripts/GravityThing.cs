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
            Physics2D.gravity = ((-groundCastRight.normal + -groundCastLeft.normal) * 0.5f) * 9.8f;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, -Physics2D.gravity),
            Time.fixedDeltaTime * rotationAdjustmentSpeed);

        if (showDebug) Debug.DrawRay(transform.position, Physics2D.gravity, Color.cyan);
    }
}