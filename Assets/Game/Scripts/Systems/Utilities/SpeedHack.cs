using UnityEngine;

public class SpeedHack : MonoBehaviour
{
    private float originalTimeScale;

    private void Awake() => originalTimeScale = Time.timeScale;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) Time.timeScale = originalTimeScale * 2;
        if (Input.GetKeyUp(KeyCode.LeftShift)) Time.timeScale = originalTimeScale;
    }
}