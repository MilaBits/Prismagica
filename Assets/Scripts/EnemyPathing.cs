using System;
using Shapes;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    private LayerMask targetLayer;

    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        RaycastHit2D enemySpotted =
            Physics2D.CircleCast(transform.position, detectionRadius, Vector2.up, detectionRadius, targetLayer);

        if (enemySpotted)
        {
            Vector3.MoveTowards(transform.position, enemySpotted.transform.position, .1f);
        }
    }

    private void OnDrawGizmos()
    {
        Draw.LineGeometry = LineGeometry.Billboard;
        Draw.DiscRadiusSpace = ThicknessSpace.Meters;
        Draw.Ring(transform.position, Vector3.forward, detectionRadius, .01f, Color.red);
    }
}