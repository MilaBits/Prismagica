using System;
using Game.Scripts.Systems.RuntimeSet;
using Shapes;
using UnityEngine;

public class RegisterToggleLines : MonoBehaviour
{
    [SerializeField] private GameObject LinesA = default;
    [SerializeField] private GameObject LinesB = default;
    [SerializeField] private GameObjectRuntimeSet linesARuntimeSet = default;
    [SerializeField] private GameObjectRuntimeSet linesBRuntimeSet = default;

    private void OnEnable()
    {
        linesARuntimeSet.AddToSet(LinesA);
        linesBRuntimeSet.AddToSet(LinesB);

        LinesA.SetActive(false);
        LinesB.SetActive(false);
    }

    private void OnDisable()
    {
        linesARuntimeSet.RemoveFromSet(LinesA);
        linesBRuntimeSet.RemoveFromSet(LinesB);
    }

    //TODO: Temporary while lines are all separate
    private void OnDrawGizmos()
    {
        for (int i = 0; i < LinesA.transform.childCount; i++)
            Draw.Text(LinesA.transform.GetChild(i).position, "A", Color.red);

        for (int i = 0; i < LinesB.transform.childCount; i++)
            Draw.Text(LinesB.transform.GetChild(i).position, "B", Color.blue);
    }
}