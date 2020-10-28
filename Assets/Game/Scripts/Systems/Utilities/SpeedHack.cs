using UnityEngine;

public class SpeedHack : MonoBehaviour
{
    private float originalTimeScale;

    private void Awake()
    {
        originalTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Speedhack On");
            Time.timeScale = originalTimeScale * 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Speedhack Off");
            Time.timeScale = originalTimeScale;
        }
    }
}