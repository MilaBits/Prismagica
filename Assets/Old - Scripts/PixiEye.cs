using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixiEye : MonoBehaviour
{
    [SerializeField]
    private Transform wings = default;

    private Camera gameCamera;

    private void Awake()
    {
        gameCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 5));

        Vector3 relativePos = transform.position - mousePos;
        float angle = (Mathf.Rad2Deg * Mathf.Atan2(relativePos.y, relativePos.x)) + 180;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (gameCamera.WorldToScreenPoint(transform.position).x > Input.mousePosition.x)
        {
            wings.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            wings.localScale = new Vector3(1, 1, 1);
        }
    }
}