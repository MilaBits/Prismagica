using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlamelingCollector : MonoBehaviour
{
    [SerializeField] private int collectedFlamelings;
    public Stack<Flameling> flamelings = new Stack<Flameling>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flameling"))
        {
            Flameling flameling = other.GetComponent<Flameling>();
            if (flameling.Collectable)
            {
                flameling.MarkCollected();
                AbsorbFlameling(flameling);
            }
        }
    }

    private void AbsorbFlameling(Flameling flameling)
    {

        collectedFlamelings++;
        flameling.SetHoverTarget(transform, .2f, .2f);
        flamelings.Push(flameling);
        // Destroy(flameling.gameObject);
    }

    public int Drain(Transform target, int amount, float speed)
    {
        collectedFlamelings -= amount;
        if (collectedFlamelings < 0)
        {
            amount = amount - Mathf.Abs(collectedFlamelings);
            collectedFlamelings = 0;
        }

        for (int i = 0; i < amount; i++)
        {
            Flameling flameling = flamelings.Pop();
            flameling.SetHoverTarget(target, 0f, speed);
            Destroy(flameling.gameObject, speed);
        }

        return amount;
    }
}