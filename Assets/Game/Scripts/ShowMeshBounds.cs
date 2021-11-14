using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class ShowMeshBounds : MonoBehaviour
{
    public Color color = Color.green;

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;

    private float borderWidth = .02f;

    void Update()
    {
        CalcPositons();
        DrawBox();
    }

    void CalcPositons()
    {
        // Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;

        Bounds bounds;
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        if (bc != null)
            bounds = bc.bounds;
        else
            return;

        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z); // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z); // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z); // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z); // Front bottom right corner
    }

    void DrawBox()
    {
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, color);
        Debug.DrawLine(v3FrontTopLeft - Vector3.up * borderWidth, v3FrontTopRight - Vector3.up * borderWidth, color);
        
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, color);
        Debug.DrawLine(v3FrontTopRight - Vector3.right * borderWidth, v3FrontBottomRight - Vector3.right * borderWidth, color);


        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, color);
        Debug.DrawLine(v3FrontBottomRight- Vector3.down * borderWidth, v3FrontBottomLeft- Vector3.down * borderWidth, color);

        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, color);
        Debug.DrawLine(v3FrontBottomLeft- Vector3.left * borderWidth, v3FrontTopLeft- Vector3.left * borderWidth, color);
    }
}