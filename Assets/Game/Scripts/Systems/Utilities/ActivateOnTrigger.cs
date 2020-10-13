using UnityEngine;

public class ActivateOnTrigger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour target;
    [SerializeField] private string triggerTag;
    [SerializeField] private bool active;
    [SerializeField] private bool onTriggerEnter;
    [SerializeField] private bool onTriggerStay;
    [SerializeField] private bool onTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter && other.CompareTag(triggerTag)) target.enabled = active;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (onTriggerStay && other.CompareTag(triggerTag)) target.enabled = active;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (onTriggerExit && other.CompareTag(triggerTag)) target.enabled = active;
    }
}