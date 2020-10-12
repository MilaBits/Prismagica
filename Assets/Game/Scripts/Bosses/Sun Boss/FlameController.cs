using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Sun_Boss
{
    public class FlameController : MonoBehaviour
    {
        [SerializeField] private Flame flamePrefab;
        private GameObject flameRing;
        private GameObject flamePoolObject;

        [SerializeField] private float flameRadius;
        [SerializeField] private int flameCount;
        [SerializeField] private float flameSpeed;
        [SerializeField, Range(-20, 20)] private float flameRotationSpeed;

        private List<Flame> circleFlames = new List<Flame>();

        [SerializeField] private Queue<Flame> flamePool = new Queue<Flame>();

        [SerializeField] private List<FlameWave> Patterns = new List<FlameWave>();

        public void PoolFlame(Flame flame)
        {
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
                Flame flame = Instantiate(flamePrefab.gameObject, flameRing.transform).GetComponent<Flame>().Init(false, 0,this,0);
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

        private IEnumerator PlayPattern()
        {
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
                        // StartCoroutine(PoolFlameDelayed(3f, flame));
                    }
                }
            }
        }

        private void Update()
        {
            flameRing.transform.Rotate(new Vector3(0, 0, flameRotationSpeed * Time.deltaTime));

            // if (fire)
        }
    }
}