using UnityEngine;

public class ActivateOnTrigger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour target = default;
    [SerializeField] private string triggerTag = default;
    [SerializeField] private bool triggerOnce = default;
    private bool _done = false;
    [SerializeField] private bool activateOnTrigger = default;
    [Header("Triggers")] [SerializeField] private bool onTriggerEnter = default;
    [SerializeField] private bool onTriggerStay = default;
    [SerializeField] private bool onTriggerExit = default;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter && other.CompareTag(triggerTag) && !_done)
        {
            target.enabled = activateOnTrigger;
            if (triggerOnce) _done = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (onTriggerStay && other.CompareTag(triggerTag) && !_done)
        {
            target.enabled = activateOnTrigger;
            if (triggerOnce) _done = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (onTriggerExit && other.CompareTag(triggerTag) && !_done)
        {
            target.enabled = activateOnTrigger;
            if (triggerOnce) _done = true;
        }
    }
}