using UnityEngine;

namespace DefaultNamespace
{
    public class FlyingPather : EnemyPather
    {
        public override void Path(Collider2D target)
        {
            if (target)
                transform.position = Vector2.MoveTowards(
                    transform.position, target.transform.position, speed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(
                    transform.position, origin, speed * Time.deltaTime);
        }
    }
}