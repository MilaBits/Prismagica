using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses.Sun_Boss
{
    public class Flame : MonoBehaviour
    {
        [SerializeField] private bool move = false;
        [SerializeField] private float speed = 5f;
        private FlameController controller;
        [Header("Lifetime")] [SerializeField] private float lifetime = default;
        [SerializeField] private float timeLived = default;
        [SerializeField] private LayerMask hitMask = default;

        [Header("Flameling")] [SerializeField] private float flamelingChance = default;
        [SerializeField] private GameObject flameling = default;
        private Renderer flameRenderer;
        private Rigidbody2D rb;

        private void Awake()
        {
            flameRenderer = GetComponent<Renderer>();
            rb = GetComponent<Rigidbody2D>();
            rb.useFullKinematicContacts = true;
        }

        public Flame Init(bool move, float speed, FlameController controller, float lifeTime)
        {
            this.move = move;
            this.speed = speed;
            this.controller = controller;
            lifetime = lifeTime;
            return this;
        }

        private void OnEnable()
        {
            timeLived = 0;
        }

        private void Update()
        {
            // If it's lifetime is over, return to flame pool.
            if (timeLived > lifetime)
            {
                controller.PoolFlame(this);
                return;
            }

            if (lifetime > 0) timeLived += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (move)
            {
                // transform.Translate(Vector2.right * (speed * Time.deltaTime), Space.Self);
                rb.MovePosition(transform.position +
                                transform.TransformDirection(Vector2.right * (speed * Time.deltaTime)));
            }
        }

        private void SpawnFlameling()
        {
            if (Random.Range(1, 101) <= flamelingChance)
            {
                Instantiate(flameling,
                    transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(0, 1f),
                    Quaternion.identity);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (flameRenderer.isVisible && hitMask == (hitMask | (1 << collision.gameObject.layer)))
            {
                SpawnFlameling();
                controller.PoolFlame(this);
            }
        }
    }
}