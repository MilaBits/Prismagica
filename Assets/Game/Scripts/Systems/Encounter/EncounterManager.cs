using System.Collections.Generic;
using Systems.Enemy;
using Shapes;
using UnityEngine;

namespace Systems.Encounter
{
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] private Encounter encounter = default;
        [SerializeField] private Vector2 gridOffset = default;
        [SerializeField] private Vector2 gridSpacing = default;
        [SerializeField] private float floorOffset = .1f;

        [SerializeField] private List<Enemy> enemies = new List<Enemy>();

        public void Init(Encounter inputEncounter)
        {
            encounter = inputEncounter;
            Init();
        }

        [ContextMenu("Init")]
        private void Init()
        {
            foreach (PositionedEnemy positionedEnemy in encounter.Enemies)
            {
                Enemy enemy = Instantiate(
                    positionedEnemy.enemy.gameObject,
                    CalculateGridPosition(positionedEnemy),
                    Quaternion.identity).GetComponent<Enemy>();

                if (enemy.transform.position.y < 0)
                    enemy.transform.localScale =
                        new Vector2(enemy.transform.localScale.x, -enemy.transform.localScale.y);
                enemies.Add(enemy);
            }
        }

        private Vector2 CalculateGridPosition(PositionedEnemy positionedEnemy)
        {
            float offset = positionedEnemy.Position.y > 0 ? floorOffset : -floorOffset;
            return gridOffset + new Vector2(positionedEnemy.Position.x * gridSpacing.x,
                offset + positionedEnemy.Position.y * gridSpacing.y);
        }

        private void OnDrawGizmos()
        {
            Draw.DiscRadiusSpace = ThicknessSpace.Meters;

            foreach (PositionedEnemy positionedEnemy in encounter.Enemies)
            {
                Draw.Disc(CalculateGridPosition(positionedEnemy), Vector3.forward, .05f, Color.cyan);
            }
        }
    }
}