using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlamelingCollector : MonoBehaviour
{
    [SerializeField] private int collectedFlamelings;
    [SerializeField] private TextMeshProUGUI counter;


    private void Update() => counter.text = collectedFlamelings.ToString();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flameling"))
        {
            Flameling flameling = other.GetComponent<Flameling>();
            if (flameling.Collectable)
            {
                flameling.MarkCollected();
                StartCoroutine(AbsorbFlameling(flameling));
            }
        }
    }

    private IEnumerator AbsorbFlameling(Flameling flameling)
    {
        Vector2 start = flameling.transform.position;

        float duration = .75f;
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            flameling.transform.position = Vector3.LerpUnclamped(start, transform.position, elapsedTime / duration);
            yield return null;
        }

        collectedFlamelings++;
        Destroy(flameling.gameObject);
    }

    public int Drain(int amount)
    {
        collectedFlamelings -= amount;
        if (collectedFlamelings < 0)
        {
            amount = amount - Mathf.Abs(collectedFlamelings);
            collectedFlamelings = 0;
        }

        return amount;
    }
}