using Extensions;
using UnityEngine;

namespace Systems.Player
{
    public class FlippedCharacter : MonoBehaviour
    {
        private Coroutine coroutine;
        private bool fadeRunning;

        [SerializeField] private Animator animator = default;

        [SerializeField] private Color flipVisibleColor = default;

        private float horizontalFlipScale;

        [SerializeField, VectorLabels("map", "to")]
        private Vector2 minDistToAlphaMap = default;

        [SerializeField, VectorLabels("map", "to")]
        private Vector2 maxDistToAlphaMap = default;

        public float distanceFromCharacter;

        private static readonly int Walking = Animator.StringToHash("Walking");

        private SpriteRenderer[] bodyPartRenderers;

        [SerializeField] private MirrorFadeCollider circleFadeCollider = default;

        private void Start()
        {
            horizontalFlipScale = animator.transform.localScale.x;
            bodyPartRenderers = animator.transform.GetComponentsInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAlpha();
        }

        private void UpdateAlpha()
        {
            float alpha = distanceFromCharacter.Remap(minDistToAlphaMap.x, maxDistToAlphaMap.x, minDistToAlphaMap.y,
                maxDistToAlphaMap.y);
            alpha -= .5f - circleFadeCollider.colliderDistance;

            foreach (SpriteRenderer bodyPartRenderer in bodyPartRenderers)
            {
                bodyPartRenderer.color = new Color(flipVisibleColor.r, flipVisibleColor.g, flipVisibleColor.b, alpha);
            }
        }

        public void Animate(MoveDirection move)
        {
            animator.SetBool(Walking, move != MoveDirection.None);
            if (move == MoveDirection.None) return;

            Vector3 scale = animator.transform.localScale;
            if (move == MoveDirection.Right)
                animator.transform.localScale = new Vector3(horizontalFlipScale, scale.y, scale.z);
            else
                animator.transform.localScale = new Vector3(-horizontalFlipScale, scale.y, scale.z);
        }
    }
}