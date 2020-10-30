using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlamelingSpawnZone : MonoBehaviour
{
    [Header("References"), SerializeField] private Flameling flamelingPrefab;
    [SerializeField] private Collider2D zone;

    [Header("Settings"), SerializeField] private float spawnInterval;
    [SerializeField] private float Deviance;
    [Space, SerializeField] private float spawnCount;
    [SerializeField] private bool continuous;

    private int totalSpawned;
    private Coroutine _coroutine;

    private void startSpawning() => _coroutine = StartCoroutine(Spawn());
    private void StopSpawning() => StopCoroutine(_coroutine);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) startSpawning();
    }

    private IEnumerator Spawn()
    {
        while (totalSpawned < spawnCount)
        {
            yield return new WaitForSeconds(spawnInterval + Random.Range(-Deviance, Deviance));
            Flameling flameling = Instantiate(flamelingPrefab, FindPointInZone(), Quaternion.identity);

            if (!continuous) totalSpawned++;
        }
    }

    private Vector3 FindPointInZone()
    {
        Vector3 point = new Vector2(
            Random.Range(zone.bounds.min.x, zone.bounds.max.x),
            Random.Range(zone.bounds.min.y, zone.bounds.max.y));

        if (zone.OverlapPoint(point)) return point;
        return FindPointInZone();
    }
}