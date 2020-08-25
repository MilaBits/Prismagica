using UnityEngine;

public class EnemyPather : MonoBehaviour
{
    protected float speed;
    protected Vector2 origin;

    public void Init(float chaseSpeed, Vector2 startPosition)
    {
        speed = chaseSpeed;
        origin = startPosition;
    }
    
    public virtual void Path(Collider2D target)
    {
        if (target)
            transform.position = Vector2.MoveTowards(
                transform.position, target.transform.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(
                transform.position, origin, speed * Time.deltaTime);
    }
}