using System;
using System.Collections;
using Systems;
using TMPro;
using UnityEngine;

public class FlowerCannon : MonoBehaviour
{
    [SerializeField, VectorLabels("Max", "Current")]
    private Vector2Int power;

    [SerializeField] private Transform pivot;
    [SerializeField] private SunBoss sun;
    [SerializeField] private Transform laser;

    [SerializeField] private Transform powerBall;
    [SerializeField] private Vector2 maxScale;
    [SerializeField] private AnimationCurve powerAddCurve;

    [SerializeField] private float drainCooldown = .1f;
    [SerializeField] private float passedTimeSinceDrain = 0;
    [SerializeField] private int drainAmount = 2;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float idleTime = 1f;
    [SerializeField] private float idleTimePassed = 0;


    private void Start()
    {
        powerBall.localScale = Vector3.zero;
    }

    [ContextMenu("Fire")]
    private void Fire()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        laser.gameObject.SetActive(true);
        sun.TakeDamage(power.y);
        int loss = power.y;
        FillPower(power.y, 0, 1f);
        power.y = 0;
        yield return new WaitForSeconds(1f);
        laser.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (idleTimePassed > idleTime && power.y >= 15)
        {
            Debug.Log("Shoot");
            StartCoroutine(Shoot());
            idleTimePassed = 0;
        }

        text.text = power.y.ToString();
        idleTimePassed += Time.deltaTime;
        passedTimeSinceDrain += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Pixi") && passedTimeSinceDrain >= drainCooldown)
        {
            FlamelingCollector collector = other.GetComponentInChildren<FlamelingCollector>();

            int gain = collector.Drain(drainAmount);
            StartCoroutine(FillPower(power.y, power.y + gain, drainCooldown));
            power.y += gain;
            if (power.y < 0) power.y = 0;


            //TODO: play animation that transfers flames to flower

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