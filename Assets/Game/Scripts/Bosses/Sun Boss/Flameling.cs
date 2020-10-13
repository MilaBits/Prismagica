using System.Collections;
using Extensions;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flameling : MonoBehaviour
{
    [SerializeField] private float deviationRange;
    private float _originalDeviationRange;

    [SerializeField] private Transform followTarget;
    private Vector2 _originalPosition;

    [SerializeField] private float hoverSpeed = .75f;
    private float _originalHoverSpeed;

    [SerializeField] private bool move;
    private Coroutine _playingCoroutine;
    private bool _running;

    private bool _collectable = true;

    [SerializeField] private Material[] colors = new Material[3];

    public bool Collectable => _collectable;

    private void Start()
    {
        GetComponent<Renderer>().material = colors[Random.Range(0, 3)];
        _originalPosition = transform.position;
        _originalDeviationRange = deviationRange;
        _originalHoverSpeed = hoverSpeed;
    }

    private IEnumerator Hover()
    {
        _running = true;
        while (move)
        {
            Vector2 start = transform.position;
            Vector2 target;
            if (followTarget != null)
            {
                target = followTarget.position.AsVector2();
            }
            else
                target = _originalPosition;

            target += new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized *
                      Random.Range(0, deviationRange);


            float duration = hoverSpeed;
            for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
            {
                transform.position = Vector3.LerpUnclamped(start, target, elapsedTime / duration);
                yield return null;
            }
        }

        _running = false;
    }

    public void SetHoverTarget(Transform target, float maxDeviation, float hoverSpeed)
    {
        followTarget = target;
        deviationRange = maxDeviation;
        this.hoverSpeed = hoverSpeed;
    }

    public void RemoveHoverTarget()
    {
        followTarget = null;
        _originalPosition = transform.position;
        deviationRange = _originalDeviationRange;
        hoverSpeed = _originalHoverSpeed;
    }

    public void MarkCollected()
    {
        _collectable = false;
    }

    private void Update()
    {
        if (move && !_running) _playingCoroutine = StartCoroutine(Hover());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Draw.LineGeometry = LineGeometry.Billboard;
        Draw.DiscRadiusSpace = ThicknessSpace.Meters;
        Draw.RingThicknessSpace = ThicknessSpace.Meters;
        Draw.Ring(_originalPosition, Vector3.forward, deviationRange, .01f, Color.gray);
    }
#endif
}