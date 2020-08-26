using System.Collections.Generic;
using UnityEngine;

namespace Systems.Parallax
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField]
        private Transform target = default;

        private Vector3 oldPos;

        [SerializeField]
        private GameObject layerPrefab = default;

        [SerializeField]
        private float frontLayers = default;

        [SerializeField]
        private List<ParallaxLayer> ParallaxLayers = default;

        private void Start()
        {
            for (int i = 0; i < ParallaxLayers.Count; i++)
            {
                ParallaxLayer parallaxLayer = ParallaxLayers[i];
                parallaxLayer.SceneObject = Instantiate(layerPrefab, gameObject.transform).GetComponent<MeshRenderer>();

                parallaxLayer.SceneObject.transform.localPosition = new Vector3(0, 0, i);
                if (i < frontLayers) parallaxLayer.SceneObject.transform.localPosition += new Vector3(0, 0, -1 - i);

                parallaxLayer.SceneObject.material = Instantiate(parallaxLayer.Material);
                parallaxLayer.SceneObject.name = parallaxLayer.name;
            }
        }

        private void Update()
        {
            transform.position = target.position + new Vector3(0, 0, -target.position.z);

            for (int i = 0; i < ParallaxLayers.Count; i++)
            {
                ParallaxLayer layer = ParallaxLayers[i];

                if (oldPos != target.position)
                {
                    layer.SceneObject.material.mainTextureOffset += (Vector2) (target.position - oldPos) * layer.Speed;
                }
            }

            oldPos = target.position;
        }
    }
}