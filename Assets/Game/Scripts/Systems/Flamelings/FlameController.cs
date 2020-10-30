using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Sun_Boss
{
    public class FlameController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Flame flamePrefab = default;
        [NonSerialized] public GameObject flameRing;
        private GameObject flamePoolObject;

        [Header("Flame Settings")] [SerializeField]
        private float flameRadius = default;

        [SerializeField] private int flameCount = default;
        [SerializeField] private float flameSpeed = default;
        [SerializeField, Range(-20, 20)] private float flameRotationSpeed = default;

        private List<Flame> circleFlames = new List<Flame>();

        [SerializeField] private Queue<Flame> flamePool = new Queue<Flame>();
        private ParticleSystem _particleSystem;

        [SerializeField] private List<FlameWave> Patterns = new List<FlameWave>();
        private bool playing;

        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public void PoolFlame(Flame flame)
        {
            _particleSystem.transform.position = flame.transform.position;
            _particleSystem.Play();

            flame.gameObject.SetActive(false);
            flamePool.Enqueue(flame);
        }

        public Flame GetFlame()
        {
            Flame flame = flamePool.Dequeue();
            flame.gameObject.SetActive(true);
            return flame;
        }

        private void Start()
        {
            flameRing = new GameObject("Flame Ring");
            flameRing.transform.SetParent(transform, false);
            for (float angle = 0; angle < 360; angle += 360f / flameCount)
            {
                float radians = angle * Mathf.Deg2Rad;
                Flame flame = Instantiate(flamePrefab.gameObject, flameRing.transform).GetComponent<Flame>()
                    .Init(false, 0, this, 0);
                flame.transform.localPosition =
                    new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians)) * flameRadius;
                flame.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                circleFlames.Add(flame);
            }

            flamePoolObject = new GameObject("Flame Pool");
            for (int i = 0; i < circleFlames.Count * Patterns.Count; i++)
            {
                PoolFlame(Instantiate(flamePrefab, flamePoolObject.transform).Init(true, flameSpeed, this, 3f));
            }
        }

        [ContextMenu("Play Pattern")]
        private void DebugPlay()
        {
            StartCoroutine(PlayPattern());
        }

        public IEnumerator FireAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Fire();
        }

        public void Fire()
        {
            if (!playing) StartCoroutine(PlayPattern());
        }

        private IEnumerator PlayPattern()
        {
            playing = true;
            foreach (FlameWave flameWave in Patterns)
            {
                yield return new WaitForSeconds(flameWave.delay);
                for (int i = 0; i < circleFlames.Count; i++)
                {
                    if (flameWave.pattern.Flames[i])
                    {
                        Flame flame = GetFlame();
                        flame.transform.position = circleFlames[i].transform.position;
                        flame.transform.rotation = circleFlames[i].transform.rotation;
                    }
                }
            }

            playing = false;
        }

        private void Update()
        {
            flameRing.transform.Rotate(new Vector3(0, 0, flameRotationSpeed * Time.deltaTime));
        }
    }
}