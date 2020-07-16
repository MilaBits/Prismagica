using UnityEngine;

// [ExecuteAlways]
public class UIController : MonoBehaviour
{
	public Camera cam;

	[SerializeField]
	private RadialColorMenu radialColorMenu;

	[SerializeField]
	private Transform hudTransform;

	private void OnEnable() => radialColorMenu.gameObject.SetActive(true);
	private void OnDisable() => radialColorMenu.gameObject.SetActive(false);

	private void Update()
	{
		radialColorMenu.DrawMenu();

		if (Input.GetKeyDown(KeyCode.Q))
		{
			radialColorMenu.ToggleSecondaryColors();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			radialColorMenu.Reveal(.5f);
		}
		
		if (Input.GetButtonDown("Interact"))
		{
			radialColorMenu.Select();
		}
	}
}