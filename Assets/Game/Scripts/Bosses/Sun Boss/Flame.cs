using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses.Sun_Boss
{
    public class Flame : MonoBehaviour
    {
        [SerializeField] private bool move = false;
        [SerializeField] private float speed = 5f;
        private FlameController controller;
        [Header("Lifetime")] [SerializeField] private float lifetime;
        [SerializeField] private float timeLived;
        [SerializeField] private LayerMask hitMask;

        [Header("Flameling")] [SerializeField] private float flamelingChance;
        [SerializeField] private GameObject flameling;
        private Renderer renderer;
        private Rigidbody2D rigidbody;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.useFullKinematicContacts = true;
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
                rigidbody.MovePosition(transform.position +
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
            if (renderer.isVisible && hitMask == (hitMask | (1 << collision.gameObject.layer)))
            {
                SpawnFlameling();
                controller.PoolFlame(this);
            }
        }
    }
}