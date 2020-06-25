using UnityEngine;
using UnityEngine.SceneManagement; //using UnityEngine.Experimental.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private string sceneToLoad = default;

        float fadeTime = 1;
        float time = 0f;
        private bool fade = false;

        [SerializeField]
        private CanvasGroup group = default;

        private Scene scene;

        public void LoadScene()
        {
            scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            fade = true;
        }

        private void Update()
        {
            if (fade)
            {
                if (time <= fadeTime)
                {
                    time += Time.deltaTime;

                    group.alpha = Mathf.Lerp(1, 0, time / fadeTime);

                    Debug.Log(group.alpha);
                    return;
                }

                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}