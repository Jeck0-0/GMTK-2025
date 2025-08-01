using UnityEngine;

public class Player : Unit
{
    public static Player instance;

    protected override void Awake()
    {
        if (instance)
        {
            Debug.LogError("Duplicate player instance");
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        base.Awake();
    }

    private void Update()
    {
        Vector2 mouseWorldPos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition).XY();
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        weapon.transform.right = direction;
        FlipWeaponVisuals(direction.x);

        if (Input.GetMouseButton(0))
        TryAttacking();
    }
    private void FlipWeaponVisuals(float directionX)
    {
        Vector3 localScale = weapon.transform.localScale;

        if (directionX < 0)
        localScale.y = -Mathf.Abs(localScale.y);
        else
        localScale.y = Mathf.Abs(localScale.y);

        weapon.transform.localScale = localScale;
    }
    public override void Damage(float damage)
    {
        if (isDead || !isVulnerable)
        return;

        Shaker.Instance.ShakeCamera(1.7f, 0.6f);
        PostFXManager.Instance.DamageFX();
        currentHealth -= damage;

        if (currentHealth <= 0)
        Die();
    }
    public override void Die()
    {
        gameObject.SetActive(false);
    }
}
