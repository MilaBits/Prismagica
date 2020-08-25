using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Encounter", menuName = "Prismagica/New Encounter", order = 0)]
    public class Encounter : ScriptableObject
    {
        public List<PositionedEnemy> Enemies;
    }
}