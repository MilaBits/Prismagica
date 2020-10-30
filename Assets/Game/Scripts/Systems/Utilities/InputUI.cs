using UnityEngine;
using UnityEngine.UI;

public class InputUI : MonoBehaviour
{
    [SerializeField] private Image WKey = default;
    [SerializeField] private Image AKey = default;
    [SerializeField] private Image SKey = default;
    [SerializeField] private Image DKey = default;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) WKey.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.A)) AKey.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.S)) SKey.color = Color.gray;
        if (Input.GetKeyDown(KeyCode.D)) DKey.color = Color.gray;

        if (Input.GetKeyUp(KeyCode.W)) WKey.color = Color.white;
        if (Input.GetKeyUp(KeyCode.A)) AKey.color = Color.white;
        if (Input.GetKeyUp(KeyCode.S)) SKey.color = Color.white;
        if (Input.GetKeyUp(KeyCode.D)) DKey.color = Color.white;
    }
}