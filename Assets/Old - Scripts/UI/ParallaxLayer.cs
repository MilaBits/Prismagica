using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Alchemage/ParallaxLayer")]
    public class ParallaxLayer : ScriptableObject
    {
        public float Speed;

        public Material Material;

        public MeshRenderer SceneObject;

        public Vector3 Offset;
    }
}