using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Loopbullet : Resetable
{
    [ReadOnly] public Targetable shotBy;
    [ReadOnly] public float speed;
    public float damage;

    [SerializeField] GameObject impactEffect;
    [SerializeField] float timeToChangeSprite = 0.5f;
    [SerializeField] Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float spriteTimer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
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
        ChangeSprite();
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
        if (collision.TryGetComponent(out ShootButton SB))
        {
            SB.Interact();
            Impact(collision, false);
        }

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
            Impact(collision, true);
            return;
        }

        Impact(collision, false);

    }

    //Inheriting classes can override this to have different
    //impact behaviors (such as bouncing on walls, or piercing enemies)
    public void Impact(Collider2D collision, bool hitEnemy)
    {
        if (impactEffect)
        Instantiate(impactEffect, transform.position, transform.rotation);

        if (hitEnemy) return;

        enabled = false;
        Destroy(gameObject);
    }
    private void ChangeSprite()
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null) return;

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= timeToChangeSprite)
        {
            spriteTimer = 0f;
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
    public override void OnReset()
    {
        Destroy(gameObject);
    }
}