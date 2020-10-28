using Game.Scripts.Systems.Collectables;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectableObject : MonoBehaviour
{
    [SerializeField] private Collectable collectabe;
}