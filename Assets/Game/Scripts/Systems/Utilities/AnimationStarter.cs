using UnityEngine;

namespace Systems.Utilities
{
    public class AnimationStarter : MonoBehaviour
    {
        [SerializeField] private bool randomStartFrame = default;

        void Start()
        {
            if (randomStartFrame)
            {
                Animator anim = GetComponent<Animator>();
                AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
                anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
            }
        }
    }
}