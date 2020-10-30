using Systems.Player;
using Game.Scripts.Systems.RuntimeSet;
using UnityEngine;

public class ToggleLine : MonoBehaviour
{
    [SerializeField] private ToggleLine OtherLine = default;
    private Collider2D col;
    [SerializeField] private GameObjectRuntimeSet PlayerRuntimeSet = default;

    [SerializeField] private PlayerMovement playerMovement = default;

    private void Start()
    {
        col = GetComponentInChildren<Collider2D>();
        playerMovement = PlayerRuntimeSet.GetItemAtIndex(0).GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Entered, toggling " + OtherLine.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            OtherLine.Toggle(false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("Exited, toggling " + OtherLine.gameObject.name);
        if (other.gameObject.CompareTag("Player")) OtherLine.Toggle(true);
    }

    public void Toggle(bool toggle)
    {
        Debug.Log("toggled: " + !col.enabled);
        col.enabled = toggle;
    }
}