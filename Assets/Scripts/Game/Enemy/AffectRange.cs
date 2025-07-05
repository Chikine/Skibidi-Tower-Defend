using UnityEngine;

public class AffectRange : MonoBehaviour
{
    private Enemy enemy;
    private CircleCollider2D circleCollider;
    public float range;
    private bool freezer;
    public void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = range;
        if(enemy.isType("freezer"))
        {
            freezer = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.TryGetComponent<Tower>(out Tower tower))
        {
            if(freezer && !tower.resistFreeze) tower.isFreeze = enemy.isTypeActivate("freezer") && !enemy.isBurn;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Tower tower = other.GetComponent<Tower>();
        if (tower != null)
        {
            tower.isFreeze = false;
        }
    }
}
