using System.Collections;
using Systems;
using Bosses.Sun_Boss;
using UnityEngine;

public class SunBoss : MonoBehaviour
{
    [SerializeField] private Transform HealthCore;
    private FlameController flameController;

    [SerializeField, VectorLabels("Max", "Current")]
    private Vector2Int health;

    [SerializeField] private AnimationCurve damageAnimationCurve;


    private void Awake() => flameController = GetComponent<FlameController>();

    private void OnEnable()
    {
        health.y = health.x;
        StartCoroutine(flameController.FireAfterDelay(1f));
        StartCoroutine(TakeDamageVisualEffect(0, health.x, .75f));
    }

    [ContextMenu("Test Take Damage")]
    public void TestTakeDamage() => TakeDamage(25);

    public void TakeDamage(int damage)
    {
        int resultHealth = health.y - damage;
        StartCoroutine(TakeDamageVisualEffect(health.y, resultHealth, 1f));
        health.y = resultHealth;
        StartCoroutine(flameController.FireAfterDelay(1f));
    }


    private IEnumerator TakeDamageVisualEffect(int currentHealth, int targetHealth, float duration)
    {
        Vector3 startScale = Vector3.one * ((float) currentHealth / health.x);
        Vector3 endScale = Vector3.one * ((float) targetHealth / health.x);
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            HealthCore.localScale = Vector3.LerpUnclamped(
                startScale,
                endScale,
                damageAnimationCurve.Evaluate(elapsedTime / duration));
            yield return null;
        }

        HealthCore.localScale = endScale;
    }
}