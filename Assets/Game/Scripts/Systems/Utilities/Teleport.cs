using Shapes;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform target = default;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.SetPositionAndRotation(target.position, target.rotation);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!target) return;

        Draw.Line(target.transform.position + target.transform.TransformDirection(Vector3.left * .2f),
            target.transform.position + target.transform.TransformDirection(Vector3.right * .2f), Color.cyan);
        Draw.Line(target.transform.position,
            target.transform.position + target.transform.TransformDirection(Vector3.up) * .5f, Color.red);

        Draw.LineDashed(transform.position, target.transform.position, DashStyle.DefaultDashStyleLine, Color.gray);
    }
#endif
}