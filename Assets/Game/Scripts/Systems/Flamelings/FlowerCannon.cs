using System.Collections;
using Systems;
using Game.Scripts.Systems.HealthSystem;
using UnityEngine;

public class FlowerCannon : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Health targetHealth;

    [SerializeField] private Transform laser = default;
    [SerializeField] private Transform powerBall = default;

    [Header("Settings")] [SerializeField, VectorLabels("Max", "Current")]
    private Vector2Int power = default;

    [SerializeField] private AnimationCurve powerAddCurve = default;
    [SerializeField] private int minimumToFire = 5;
    [SerializeField] private Vector2 maxScale = default;

    [Space] [SerializeField] private float timeBetweenDrains = .1f;
    private float passedTimeSinceDrain = 0;
    [SerializeField] private int drainSize = 2;
    [Space] [SerializeField] private float fireDelay = 2f;
    private float idleTimePassed = 0;

    private void Start() => powerBall.localScale = Vector3.zero;

    [ContextMenu("Fire")]
    private void Fire() => StartCoroutine(Shoot());

    private IEnumerator Shoot()
    {
        laser.gameObject.SetActive(true);
        targetHealth.TakeDamage(power.y);
        FillPower(power.y, 0, 1f);
        power.y = 0;
        yield return new WaitForSeconds(1f);
        laser.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (idleTimePassed > fireDelay && power.y >= minimumToFire && targetHealth)
        {
            StartCoroutine(Shoot());
            idleTimePassed = 0;
        }

        idleTimePassed += Time.deltaTime;
        passedTimeSinceDrain += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (passedTimeSinceDrain >= timeBetweenDrains && other.CompareTag("Flameling"))
        {
            Flameling flameling = other.GetComponent<Flameling>();
            
            if (flameling.Consumed) return;
            
            flameling.Consume(transform, .5f);
            StartCoroutine(FillPower(power.y, power.y + 1, timeBetweenDrains));
            power.y += 1;
            Debug.Log(power.y);


            passedTimeSinceDrain = 0;
            idleTimePassed = 0;
        }
    }

    private IEnumerator FillPower(int currentPower, int targetPower, float duration)
    {
        Vector3 startScale = maxScale * ((float) currentPower / power.x);
        Vector3 endScale = maxScale * ((float) targetPower / power.x);
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            powerBall.localScale = Vector3.LerpUnclamped(
                startScale,
                endScale,
                powerAddCurve.Evaluate(elapsedTime / duration));
            yield return null;
        }

        powerBall.localScale = endScale;
    }
}