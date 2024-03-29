﻿using System;
using System.Collections;
using Systems.Utilities;
using Shapes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.Encounter
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = default;
        [SerializeField] private LayerMask targetLayer = default;

        [SerializeField] private EnemyPather Pather = default;
        [SerializeField] private float chaseSpeed = .1f;

        void Awake() => Pather.Init(chaseSpeed, transform.position);

        private void OnEnable() => GetComponentInChildren<TriggerForwarder>().enterEvent.AddListener(StartEncounter);

        private void OnDisable() =>
            GetComponentInChildren<TriggerForwarder>().enterEvent.RemoveListener(StartEncounter);

        void Update()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);

            Pather.Path(hit);
        }

        private void StartEncounter(Collider2D other)
        {
            Debug.Log("*Final Fantasy 10 Battle Theme*\nDdu du duuu dududuud uduuuuu");
            StartCoroutine(LoadBattleScene());
        }

        private IEnumerator LoadBattleScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Draw.LineGeometry = LineGeometry.Billboard;
            Draw.DiscRadiusSpace = ThicknessSpace.Meters;
            Draw.RingThicknessSpace = ThicknessSpace.Meters;
            Draw.Ring(transform.position, Vector3.forward, detectionRadius, .01f, Color.red);
        }
#endif
    }
}