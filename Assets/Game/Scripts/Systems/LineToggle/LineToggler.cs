using Game.Scripts.Systems.LineToggle;
using Game.Scripts.Systems.RuntimeSet;
using Shapes;
using UnityEngine;

public class LineToggler : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSet targetA = default;
    [SerializeField] private GameObjectRuntimeSet targetB = default;

    [SerializeField] private LineToggleTrigger TriggerA = default;
    [SerializeField] private LineToggleTrigger TriggerB = default;

    private bool ATriggerState;
    private bool BTriggerState;

    private void OnEnable()
    {
        TriggerA.entered.AddListener(delegate { Triggered(Side.A); });
        TriggerB.entered.AddListener(delegate { Triggered(Side.B); });
        TriggerA.exited.AddListener(delegate { Exited(Side.A); });
        TriggerB.exited.AddListener(delegate { Exited(Side.B); });
    }

    private void OnDisable()
    {
        TriggerA.entered.RemoveListener(delegate { Triggered(Side.A); });
        TriggerB.entered.RemoveListener(delegate { Triggered(Side.B); });
        TriggerA.exited.RemoveListener(delegate { Exited(Side.A); });
        TriggerB.exited.RemoveListener(delegate { Exited(Side.B); });
    }

    private void Triggered(Side side)
    {
        UpdateState(side, true);
        if (ATriggerState && !BTriggerState) ToggleSides(Side.A);
        else if (BTriggerState && !ATriggerState) ToggleSides(Side.B);
    }

    private void Exited(Side side)
    {
        UpdateState(side, false);
    }

    private void UpdateState(Side side, bool state)
    {
        switch (side)
        {
            case Side.A:
                ATriggerState = state;
                break;
            case Side.B:
                BTriggerState = state;
                break;
            case Side.None:
                break;
        }
    }

    private void ToggleSides(Side side)
    {
        switch (side)
        {
            case Side.None:
                targetA.SetActive(false);
                targetB.SetActive(false);
                break;
            case Side.A:
                targetA.SetActive(true);
                targetB.SetActive(false);
                break;
            case Side.B:
                targetA.SetActive(false);
                targetB.SetActive(true);
                break;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Draw.CuboidSizeSpace = ThicknessSpace.Meters;

        Draw.Cuboid(TriggerA.transform.position, transform.rotation, TriggerA.triggerCollider.size,
            Color.red);
        Draw.Text(TriggerA.transform.position, "A", Color.white);
        Draw.Cuboid(TriggerB.transform.position, transform.rotation, TriggerB.triggerCollider.size,
            Color.blue);
        Draw.Text(TriggerB.transform.position, "B", Color.white);
    }
#endif

    private enum Side
    {
        None,
        A,
        B
    }
}