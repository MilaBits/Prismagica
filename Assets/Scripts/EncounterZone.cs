using Shapes;
using UnityEngine;

public class EncounterZone : MonoBehaviour
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private EnemyPather Pather;
    [SerializeField] private float chaseSpeed = .1f;

    void Awake() => Pather.Init(chaseSpeed, transform.position);

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);

        Pather.Path(hit);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Draw.LineGeometry = LineGeometry.Billboard;
        Draw.DiscRadiusSpace = ThicknessSpace.Meters;
        Draw.RingThicknessSpace = ThicknessSpace.Meters;
        Draw.Ring(transform.position, Vector3.forward, detectionRadius, .01f, Color.red);
    }
#endif
}