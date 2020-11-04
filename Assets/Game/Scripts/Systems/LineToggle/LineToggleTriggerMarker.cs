using System;
using System.Linq;
using Systems.Player;
using Flask;
using Game.Scripts.Systems.LineToggle;
using Game.Scripts.Systems.RuntimeSet;
using Shapes;
using UnityEngine;

public class LineToggleTriggerMarker : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSet playerSet;
    private Transform player;

    [SerializeField] private float range = 5f;

    private DTweenVector2 _position;
    [SerializeField] private float springStrength;

    [SerializeField] private LayerMask targetLayers;

    private LineToggleTrigger closestLineToggleTrigger;

    private void Awake()
    {
        player = playerSet.GetItemAtIndex(0).transform;
        _position = new DTweenVector2(player.position, springStrength);
    }

    private void Update()
    {
        Collider2D[] closeColliders = Physics2D
            .OverlapCircleAll(player.transform.position, range, targetLayers)
            .Where(x => x.GetComponent<LineToggleTrigger>())
            .ToArray();
        if (closeColliders.Length > 0)
            closestLineToggleTrigger = GetClosestCollider(player.transform.position, closeColliders)
                .GetComponent<LineToggleTrigger>();

        _position.omega = springStrength;
        _position.Step(closestLineToggleTrigger.transform.position);
        transform.position = _position.position;
    }

    Collider2D GetClosestCollider(Vector2 centerPosition, Collider2D[] colliders)
    {
        float bestDistance = 99999.0f;
        Collider2D bestCollider = null;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(centerPosition, collider.transform.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestCollider = collider;
            }
        }

        return bestCollider;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Draw.LineGeometry = LineGeometry.Billboard;
        Draw.DiscRadiusSpace = ThicknessSpace.Meters;
        Draw.RingThicknessSpace = ThicknessSpace.Meters;
        Draw.Ring(player.transform.position, Vector3.forward, range, .01f, Color.gray);

        if (closestLineToggleTrigger)
            Draw.Line(transform.position, closestLineToggleTrigger.transform.position, Color.green);
    }
#endif
}