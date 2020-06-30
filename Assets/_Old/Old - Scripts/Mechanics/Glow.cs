using System.Collections;
using UnityEngine;

namespace Mechanics
{
    public class Glow : MonoBehaviour
    {
        private SpriteRenderer glow;

        [SerializeField]
        private bool lightable = true;

        [SerializeField]
        private Color color = default;

        private void Start()
        {
            glow = transform.GetChild(0).GetComponent<SpriteRenderer>();
            glow.color = color;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInChildren<Lighter>()) Activate();
        }

        public void Activate()
        {
//        glow.gameObject.SetActive(true);
            FadeGlow(true, .1f);
        }

        public void Deactivate()
        {
            FadeGlow(false, .1f);
//        glow.gameObject.SetActive(false);
        }

        private IEnumerator FadeGlow(bool active, float time)
        {
            Color endColor;
            if (active)
            {
                endColor = new Color(color.r, color.g, color.b, 1);
                glow.gameObject.SetActive(true);
                Debug.Log(gameObject.name + " activated");
            }
            else
            {
                endColor = new Color(color.r, color.g, color.b, 0);
            }

            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                glow.color = Color.Lerp(glow.color, endColor, (elapsedTime / time));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            glow.color = endColor;
            if (!active)
            {
                glow.gameObject.SetActive(false);
                Debug.Log(gameObject.name + " activated");
            }

            yield return new WaitForEndOfFrame();
        }
    }
}