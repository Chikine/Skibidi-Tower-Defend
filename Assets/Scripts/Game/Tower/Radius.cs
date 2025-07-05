using UnityEngine;

public class Radius : MonoBehaviour
{
    private Tower tower;
    private CircleCollider2D circleCollider;

    public void Start()
    {
        tower = GetComponentInParent<Tower>();
        circleCollider = GetComponent<CircleCollider2D>();
        if (tower == null)
        {
            Debug.LogError("Tower is Null");
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            tower.AddEnemy(other.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            tower.RemoveEnemy(other.gameObject);
        }
    }

    public void setRadius(float range)
    {
        transform.localScale = new(range * 20, range * 20, range * 20);
        if( circleCollider != null )
        {
            circleCollider.radius = range;
        }
    }
}
