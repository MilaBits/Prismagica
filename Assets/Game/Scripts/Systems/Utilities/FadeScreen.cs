using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup ScreenFadeOutGroup = default;
    [SerializeField] private TextMeshProUGUI text = default;

    private void Awake() => ScreenFadeOutGroup = GetComponent<CanvasGroup>();

    public IEnumerator Fade(float start, float end, float duration, string text)
    {
        this.text.text = text;
        yield return StartCoroutine(Fade(start, end, duration));
    }

    public IEnumerator Fade(float start, float end, float duration)
    {
        for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
        {
            ScreenFadeOutGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        ScreenFadeOutGroup.alpha = end;
    }
}