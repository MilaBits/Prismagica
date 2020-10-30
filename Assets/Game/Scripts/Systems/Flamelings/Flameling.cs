using System.Collections;
using Systems;
using Extensions;
using Flask;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flameling : MonoBehaviour
{
    [SerializeField] private float deviationRange = default;
    private float _originalDeviationRange;

    [SerializeField] private Transform followTarget = default;
    private Vector2 _originalPosition;

    [SerializeField] private float hoverSpeed = .75f;
    private float _originalHoverSpeed;

    [SerializeField] private bool move = default;
    private Coroutine _playingCoroutine;
    private bool _running;

    private bool _collectable = true;

    [SerializeField] private Material[] colors = new Material[3];

    public bool Collectable => _collectable;
    public bool Consumed => _consumed;
    private bool _consumed;

    [Space] [SerializeField, VectorLabels("Min", "Max")]
    private Vector2 speedRange = new Vector2(1, 3);

    private float speed;

    DTweenVector2 _position = new DTweenVector2();
    private Vector2 targetPos = new Vector2();
    [SerializeField] private float targetPosInterval = 1f;
    private float targetPosPassed = 0f;

    private void Start()
    {
        GetComponent<Renderer>().material = colors[Random.Range(0, 3)];
        _originalPosition = transform.position;
        _originalDeviationRange = deviationRange;
        _originalHoverSpeed = hoverSpeed;

        GetTargetPos();
        speed = Random.Range(speedRange.x, speedRange.y);
        _position = new DTweenVector2(transform.position, speed);

        StartCoroutine(SpawnAnimation(1f, Vector3.zero, Vector3.one));
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

    public IEnumerator SpawnAnimation(float duration, Vector3 start, Vector3 end)
    {
        transform.localScale = Vector3.zero;
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / duration);
            yield return null;
        }

        transform.localScale = Vector3.one;
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
        if (move)
        {
            if (targetPosPassed > targetPosInterval)
            {
                speed = Random.Range(speedRange.x, speedRange.y);
                GetTargetPos();
                targetPosPassed = 0;
            }

            _position.omega = speed;
            _position.Step(targetPos);
            transform.position = _position.position;

            targetPosPassed += Time.deltaTime;
        }
    }

    private void GetTargetPos()
    {
        if (followTarget != null)
        {
            targetPos = followTarget.position.AsVector2();
            speed *= 3;
            targetPosPassed += Time.deltaTime;
        }
        else
        {
            targetPos = _originalPosition;
        }

        targetPos += new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized *
                     Random.Range(0, deviationRange);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Draw.LineGeometry = LineGeometry.Billboard;
        Draw.DiscRadiusSpace = ThicknessSpace.Meters;
        Draw.RingThicknessSpace = ThicknessSpace.Meters;
        Draw.Ring(_originalPosition, Vector3.forward, deviationRange, .01f, Color.gray);

        Draw.Line(transform.position, targetPos, Color.green);
    }
#endif

    public void Consume(Transform target, float duration)
    {
        _consumed = true;
        targetPos = target.position;
        targetPosInterval = duration;
        targetPosPassed = 0;
        StartCoroutine(SpawnAnimation(duration, Vector3.one, Vector3.zero));
        Destroy(gameObject, duration);
    }
}