using UnityEngine;

public class EscapeToQuit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
    }
}