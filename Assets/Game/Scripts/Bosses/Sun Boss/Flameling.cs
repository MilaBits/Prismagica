using System.Collections;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flameling : MonoBehaviour
{
    private Vector2 _originalPosition;
    [SerializeField] private float deviationRange;
    [SerializeField] private bool move;
    private bool _running;
    private bool _collectable = true;
    private Coroutine _playingCoroutine;
    public bool Collectable => _collectable;

    private void Start() => _originalPosition = transform.position;

    private IEnumerator Hover()
    {
        _running = true;
        while (move)
        {
            Vector2 start = transform.position;
            Vector2 target = _originalPosition +
                             new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized *
                             Random.Range(0, deviationRange);

            float duration = .75f;
            for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
            {
                transform.position = Vector3.LerpUnclamped(start, target, elapsedTime / duration);
                yield return null;
            }
        }

        _running = false;
    }

    public void MarkCollected()
    {
        move = false;
        _collectable = false;
        StopCoroutine(_playingCoroutine);
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