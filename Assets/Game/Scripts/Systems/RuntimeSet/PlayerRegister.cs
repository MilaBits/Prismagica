using System;
using Systems.Player;
using Game.Scripts.Systems.RuntimeSet;
using UnityEngine;

public class PlayerRegister : MonoBehaviour
{
    [SerializeField] private GameObjectRuntimeSet runtimeSet;

    private void OnEnable() => runtimeSet.AddToSet(FindObjectOfType<PlayerMovement>().gameObject);
}