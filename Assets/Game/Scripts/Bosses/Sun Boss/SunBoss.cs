using System;
using System.Collections;
using Bosses.Sun_Boss;
using Game.Scripts.Systems.HealthSystem;
using UnityEngine;

public class SunBoss : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform originalSunArt = default;

    [SerializeField] private Transform HealthCore = default;
    [SerializeField] private Health _health = default;

    private FlameController flameController;
    [Header("Settings")] [SerializeField] private AnimationCurve damageAnimationCurve = default;

    private int lastHealth;

    private void Awake()
    {
        flameController = GetComponent<FlameController>();
    }

    private void OnEnable()
    {
        _health.Death.AddListener(delegate { StartCoroutine(Death(1f, 5f)); });
        _health.HealthChanged.AddListener(delegate(int damage) { TakeDamage(damage); });

        StartCoroutine(flameController.FireAfterDelay(1f));
        StartCoroutine(TakeDamageVisualEffect(0, _health.MaxHealth, .75f));
        lastHealth = _health.MaxHealth;
    }

    private void OnDisable()
    {
        _health.Death.RemoveListener(delegate { StartCoroutine(Death(1f, 5f)); });
        _health.HealthChanged.RemoveListener(delegate(int damage) { TakeDamage(damage); });
    }

    [ContextMenu("Test Take Damage")]
    public void TestTakeDamage() => _health.TakeDamage(100);

    public void TakeDamage(int newHealth)
    {
        StartCoroutine(TakeDamageVisualEffect(lastHealth, newHealth, 1f));
        Debug.Log($"lastHealth: {lastHealth}, newHealth: {newHealth}");

        if (_health.CurrentHealth > 0) StartCoroutine(flameController.FireAfterDelay(1f));
        lastHealth = _health.CurrentHealth;
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
    }


    private IEnumerator TakeDamageVisualEffect(int currentHealth, int targetHealth, float duration)
    {
        Vector3 startScale = Vector3.one * ((float) currentHealth / _health.MaxHealth);
        Vector3 endScale = Vector3.one * ((float) targetHealth / _health.MaxHealth);
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