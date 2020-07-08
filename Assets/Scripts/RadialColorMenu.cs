using System;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using UnityEngine;

public class RadialColorMenu : MonoBehaviour
{
    [Serializable]
    public class ColorMenuItem
    {
        public Color color;

        [HideInInspector]
        public float size;

        [HideInInspector]
        public Disc visualPiece;

        public bool enabled;
    }

    [SerializeField]
    private List<ColorMenuItem> colorItems;

    void Start()
    {
        List<ColorMenuItem> items = colorItems.Where(x => x.enabled).ToList();
        
        foreach (ColorMenuItem colorMenuItem in items)
        {
            colorMenuItem.visualPiece =
                Instantiate(new GameObject(colorMenuItem.color.ToString()), transform).AddComponent<Disc>();
        }

        float size = (360f / items.Count) * Mathf.Deg2Rad;
        float currentSize = 0;
        foreach (ColorMenuItem colorMenuItem in items)
        {
            currentSize += size;
            colorMenuItem.size = size;
            colorMenuItem.visualPiece.Radius = 10f;
            colorMenuItem.visualPiece.RadiusSpace = ThicknessSpace.Noots;
            colorMenuItem.visualPiece.Thickness = 1f;
            colorMenuItem.visualPiece.ThicknessSpace = ThicknessSpace.Meters;
            colorMenuItem.visualPiece.Type = Disc.DiscType.Arc;
            colorMenuItem.visualPiece.Color = colorMenuItem.color;
            colorMenuItem.visualPiece.AngRadiansStart = currentSize;
            colorMenuItem.visualPiece.AngRadiansEnd = currentSize + size;
        }
    }

    void Update()
    {
        
    }
}