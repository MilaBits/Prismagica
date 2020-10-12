using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    [ContextMenu("Rotate With Camera")]
    void Update() => transform.rotation = Camera.main.transform.rotation;
}