using System.Collections;
using Game.Scripts.Systems.RuntimeSet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Systems.HealthSystem
{
    public class HealthUI : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform[] petals = default;

        [SerializeField] private FadeScreen fadeScreen = default;

        [SerializeField] private GameObjectRuntimeSet PlayerRuntimeSet;

        void Start()
        {
            PlayerRuntimeSet.GetItemAtIndex(0).GetComponentInChildren<Health>().HealthChanged
                .AddListener(delegate(int health) { UpdateHealthUI(health); });
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
}