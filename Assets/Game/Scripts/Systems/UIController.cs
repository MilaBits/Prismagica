using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
namespace Systems
{
    public class UIController : MonoBehaviour
    {
        public UnityEngine.Camera cam;

        [SerializeField] private RadialColorMenu radialColorMenu = default;

        [SerializeField] private FadeScreen fadeScreen = default;

        private void Start() =>
            StartCoroutine(fadeScreen.Fade(1, 0, 2f, "Thank you for trying this small demonstration :)"));

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


        private IEnumerator Fade(float start, float end, CanvasGroup group, float duration)
        {
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                group.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }

            group.alpha = end;
        }
    }
}