using System.Collections.Generic;
using Game.Scripts.Systems.RuntimeSet;
using UnityEngine;

public class RuntimeSetInitializer : MonoBehaviour
{
    public List<GameObjectRuntimeSet> RuntimeSets = new List<GameObjectRuntimeSet>();

    private void Awake()
    {
        for (int i = 0; i < RuntimeSets.Count; i++)
        {
            RuntimeSets[i].Initialize();
        }
    }
}