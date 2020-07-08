using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Alchemage/ParallaxLayer")]
[Serializable]
public class ParallaxLayer : ScriptableObject
{
    public float Speed;

    public Material Material;

    public MeshRenderer SceneObject;

    public Vector3 Offset;
}