using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [ReadOnly] public float damage;
    [ReadOnly] public float speed;
    [ReadOnly] public Targetable shotBy;
    [SerializeField] GameObject impactEffect;
    public virtual void Initialize(float damage, float speed, float duration, Targetable shotBy = null)
    {
        this.damage = damage;
        this.speed = speed;
        this.shotBy = shotBy;
        Destroy(gameObject, duration);
    }

    public void Update()
    {
        MoveUpdate();
    }

    //Inheriting classes can override this to move in a different way
    protected void MoveUpdate()
    {
        float moveby = speed * Time.deltaTime;
        transform.position += transform.right * moveby;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;*/

        if (collision.TryGetComponent(out Targetable targetable))
        {
            if (targetable == shotBy)
                return;
            
            targetable.Damage(damage);
            Impact(collision, true);
            return;
        }

        if (collision.TryGetComponent(out Projectile proj))
        {
            damage -= proj.damage;
            if(damage <= 0)
                Impact(collision, false);
            return;
        }
        
        Impact(collision, false);

    }

    //Inheriting classes can override this to have different
    //impact behaviors (such as bouncing on walls, or piercing enemies)
    public void Impact(Collider2D collision, bool hitEnemy)
    {
        enabled = false;
        if(impactEffect)
            Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
