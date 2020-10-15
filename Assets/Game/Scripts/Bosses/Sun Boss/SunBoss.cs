using System.Collections;
using Systems;
using Bosses.Sun_Boss;
using UnityEngine;

public class SunBoss : MonoBehaviour
{
    [SerializeField] private Transform originalSunArt = default;
    [SerializeField] private Transform HealthCore = default;
    private FlameController flameController;

    [SerializeField, VectorLabels("Max", "Current")]
    private Vector2Int health = default;

    [SerializeField] private AnimationCurve damageAnimationCurve = default;


    private void Awake() => flameController = GetComponent<FlameController>();

    private void OnEnable()
    {
        health.y = health.x;
        StartCoroutine(flameController.FireAfterDelay(1f));
        StartCoroutine(TakeDamageVisualEffect(0, health.x, .75f));
    }

    [ContextMenu("Test Take Damage")]
    public void TestTakeDamage() => TakeDamage(100);

    public void TakeDamage(int damage)
    {
        int resultHealth = Mathf.Clamp(health.y - damage, 0, 100);
        StartCoroutine(TakeDamageVisualEffect(health.y, resultHealth, 1f));
        health.y = resultHealth;

        if (health.y <= 0)
        {
            StartCoroutine(Death(1f, 5f));
        }
        else
        {
            StartCoroutine(flameController.FireAfterDelay(1f));
        }
    }

    private IEnumerator Death(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(TakeDamageVisualEffect(0, 300, duration * .75f));
        originalSunArt.gameObject.SetActive(true);

        gameObject.GetComponent<Renderer>().enabled = false;
        flameController.flameRing.SetActive(false);
        yield return StartCoroutine(TakeDamageVisualEffect(300, 0, duration * .25f));
        gameObject.SetActive(false);
        //gameObject.GetComponent<Renderer>().enabled = true;
        //flameController.gameObject.SetActive(true);
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