using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Transform[] petals = default;
    [SerializeField] private FadeScreen fadeScreen = default;

    void Start()
    {
        FindObjectOfType<PlayerHealth>().HealthChanged.AddListener(delegate(int arg0) { UpdateHealthUI(arg0); });
    }

    private void UpdateHealthUI(int health)
    {
        for (int i = 0; i < petals.Length; i++)
        {
            petals[i].gameObject.SetActive(health > i);
        }

        if (health <= 0)
        {
            StartCoroutine(Restart());
        }
    }

    private IEnumerator Restart()
    {
        yield return fadeScreen.Fade(0, 1, 1f, "You Died");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}